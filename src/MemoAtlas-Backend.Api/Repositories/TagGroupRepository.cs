using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class TagGroupRepository(AppDbContext db) : ITagGroupRepository
{
    public IQueryable<TagGroupWithTagsWithCountsDTO> TagGroupWithTagCountData(User user, int? tagGroupId = null)
    {
        IQueryable<TagGroup> query = db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id);

        if (tagGroupId != null)
        {
            query = query.Where(tg => tg.Id == tagGroupId);
        }

        return query
        .Include(tg => tg.Tags)
        .Select(tg => new TagGroupWithTagsWithCountsDTO
        {
            Id = tg.Id,
            Name = tg.Name,
            Color = tg.Color,
            Private = tg.Private,
            Tags = tg.Tags.Select(tag => new TagWithCountAndWithoutGroupIdDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Private = tag.Private,
                Count = tag.Memos.Count
            })
        });
    }

    public async Task<List<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsWithTagCountDataAsync(User user)
    {
        return await TagGroupWithTagCountData(user).ToListAsync();
    }

    public async Task<TagGroupWithTagsWithCountsDTO?> GetTagGroupWithTagCountDataAsync(User user, int id)
    {
        return await TagGroupWithTagCountData(user, id).FirstOrDefaultAsync();
    }

    public async Task<TagGroup?> GetTagGroupAsync(User user, int id)
    {
        return await db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync();
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