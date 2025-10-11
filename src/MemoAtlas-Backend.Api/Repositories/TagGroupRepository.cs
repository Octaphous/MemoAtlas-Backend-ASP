using System.Linq.Expressions;
using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class TagGroupRepository(AppDbContext db) : ITagGroupRepository
{
    public async Task<List<TagGroup>> GetAllTagGroupsAsync(User user)
    {
        return await db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id)
            .Include(tg => tg.Tags)
            .ToListAsync();
    }

    public async Task<TagGroup?> GetTagGroupAsync(User user, int id)
    {
        return await db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync();
    }

    public IQueryable<TagGroupWithTagsWithCountsDTO> TagGroupStatsQuery(User user, TagGroupStatsFilter filter, int? tagGroupId = null)
    {
        Expression<Func<Memo, bool>> inDateRange = m =>
            (filter.StartDate == null || m.Date >= filter.StartDate) &&
            (filter.EndDate == null || m.Date <= filter.EndDate);

        IQueryable<TagGroup> query = db.TagGroups
            .VisibleToUser(user)
            .Include(tg => tg.Tags)
            .Where(tg => tg.UserId == user.Id);

        if (tagGroupId != null)
        {
            query = query.Where(tg => tg.Id == tagGroupId);
        }

        return query.Select(tg => new TagGroupWithTagsWithCountsDTO
        {
            Id = tg.Id,
            Name = tg.Name,
            Color = tg.Color,
            Private = tg.Private,
            Tags = tg.Tags.Where(tag => tag.Memos.AsQueryable().Any(inDateRange)) // We dont need to list memos outside of the date range because count will be 0
            .Select(tag => new TagWithCountAndWithoutGroupIdDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Private = tag.Private,
                Count = tag.Memos.AsQueryable().Count(inDateRange)
            })
        });
    }

    public async Task<List<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsStatsAsync(User user, TagGroupStatsFilter filter)
    {
        return await TagGroupStatsQuery(user, filter).ToListAsync();
    }

    public async Task<TagGroupWithTagsWithCountsDTO?> GetTagGroupStatsAsync(User user, TagGroupStatsFilter filter, int id)
    {
        return await TagGroupStatsQuery(user, filter, id).FirstOrDefaultAsync();
    }

    public void AddTagGroup(TagGroup tagGroup)
    {
        db.TagGroups.Add(tagGroup);
    }

    public void DeleteTagGroup(TagGroup tagGroup)
    {
        db.TagGroups.Remove(tagGroup);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}