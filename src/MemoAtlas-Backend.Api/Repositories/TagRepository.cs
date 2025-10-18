using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class TagRepository(AppDbContext db) : ITagRepository
{
    public async Task<List<Tag>> GetAllTagsAsync(User user)
    {
        return await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id)
            .Include(t => t.TagGroup)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetTagsAsync(User user, HashSet<int> tagIds)
    {
        return await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && tagIds.Contains(t.Id))
            .Include(t => t.TagGroup)
            .ToListAsync();
    }

    public async Task<Tag?> GetTagAsync(User user, int id)
    {
        return await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .Include(t => t.Memos)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Tag>> GetTagsBySearchStringAsync(User user, string query)
    {
        return await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && EF.Functions.Like(t.Name, $"%{query}%"))
            .Include(t => t.TagGroup)
            .ToListAsync();
    }

    public void AddTag(Tag tag)
    {
        db.Tags.Add(tag);
    }

    public void DeleteTag(Tag tag)
    {
        db.Tags.Remove(tag);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}