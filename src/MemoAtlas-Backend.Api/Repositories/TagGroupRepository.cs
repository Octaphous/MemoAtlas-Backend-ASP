using MemoAtlas_Backend.Api.Data;
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