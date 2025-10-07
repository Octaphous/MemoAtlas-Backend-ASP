using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagService(AppDbContext db) : ITagService
{
    public async Task<List<Tag>> GetAllTagsAsync(User user)
    {
        List<Tag> tags = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id)
            .Include(t => t.TagGroup)
            .ToListAsync();

        return tags;
    }

    public async Task<List<Tag>> GetTagsAsync(User user, HashSet<int> tagIds)
    {
        if (tagIds.Count != tagIds.Distinct().Count())
        {
            throw new InvalidPayloadException("Some of the provided tag IDs are duplicates.");
        }

        List<Tag> tags = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && tagIds.Contains(t.Id))
            .Include(t => t.TagGroup)
            .ToListAsync();

        if (tags.Count != tagIds.Count)
        {
            throw new InvalidPayloadException("One or more of the provided tag IDs do not exist for this user.");
        }

        return tags;
    }

    public async Task<Tag> GetTagAsync(User user, int id)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .Include(t => t.Memos)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        return tag;
    }

    public async Task<Tag> CreateTagAsync(User user, TagCreateRequest body)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == body.GroupId)
            .FirstOrDefaultAsync() ?? throw new InvalidPayloadException("Specified tag group does not exist.");

        Tag tag = new()
        {
            TagGroupId = body.GroupId,
            Name = body.Name,
            Description = body.Description
        };

        db.Tags.Add(tag);
        await db.SaveChangesAsync();
        await db.Entry(tag).Reference(t => t.TagGroup).LoadAsync();

        return tag;
    }

    public async Task<Tag> UpdateTagAsync(User user, int id, TagUpdateRequest body)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .Include(t => t.Memos)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        if (body.Name != null)
        {
            tag.Name = body.Name;
        }
        if (body.Description != null)
        {
            tag.Description = body.Description;
        }

        await db.SaveChangesAsync();
        return tag;
    }

    public async Task DeleteTagAsync(User user, int id)
    {
        Tag tag = await GetTagAsync(user, id);
        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
    }
}