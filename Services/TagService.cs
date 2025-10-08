using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using MemoAtlas_Backend_ASP.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagService(AppDbContext db) : ITagService
{
    public async Task<List<Tag>> GetAllTagsAsync(User user)
    {
        List<Tag> tags = await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id)
            .Include(t => t.TagGroup)
            .ToListAsync();

        // If user is not in private mode, remove any tags that are in a private tag group
        tags = tags.Where(tag => !tag.TagGroup.Private || user.PrivateMode).ToList();

        return tags;
    }

    public async Task<List<Tag>> GetTagsAsync(User user, HashSet<int> tagIds)
    {
        if (tagIds.Count != tagIds.Distinct().Count())
        {
            throw new InvalidPayloadException("Some of the provided tag IDs are duplicates.");
        }

        List<Tag> tags = await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && tagIds.Contains(t.Id))
            .Include(t => t.TagGroup)
            .ToListAsync();

        if (tags.Count != tagIds.Count)
        {
            throw new InvalidPayloadException("One or more of the provided tag IDs do not exist for this user.");
        }

        // If user is not in private mode, remove any tags that are in a private tag group
        tags = tags.Where(tag => !tag.TagGroup.Private || user.PrivateMode).ToList();

        return tags;
    }

    public async Task<Tag> GetTagAsync(User user, int id)
    {
        Tag? tag = await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .Include(t => t.Memos)
            .FirstOrDefaultAsync();

        // If user is not in private mode, ensure the tag is not in a private tag group
        if (tag == null || (tag.TagGroup.Private && !user.PrivateMode))
        {
            throw new InvalidResourceException("Tag not found.");
        }

        // Remove all memos that are private if user is not in private mode
        tag.Memos = tag.Memos.Where(m => !m.Private || user.PrivateMode).ToList();

        return tag;
    }

    public async Task<Tag> CreateTagAsync(User user, TagCreateRequest body)
    {
        TagGroup tagGroup = await db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id && tg.Id == body.GroupId)
            .FirstOrDefaultAsync() ?? throw new InvalidPayloadException("Specified tag group does not exist.");

        Tag tag = new()
        {
            TagGroupId = body.GroupId,
            Name = body.Name,
            Description = body.Description,
            Private = body.Private
        };

        db.Tags.Add(tag);
        await db.SaveChangesAsync();
        await db.Entry(tag).Reference(t => t.TagGroup).LoadAsync();

        return tag;
    }

    public async Task<Tag> UpdateTagAsync(User user, int id, TagUpdateRequest body)
    {
        Tag? tag = await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .Include(t => t.Memos)
            .FirstOrDefaultAsync();

        // If user is not in private mode, ensure the tag is not in a private tag group
        if (tag == null || (tag.TagGroup.Private && !user.PrivateMode))
        {
            throw new InvalidResourceException("Tag not found.");
        }

        // Remove all memos that are private if user is not in private mode
        tag.Memos = tag.Memos.Where(m => !m.Private || user.PrivateMode).ToList();

        if (body.Name != null)
        {
            tag.Name = body.Name;
        }

        if (body.Description != null)
        {
            tag.Description = body.Description;
        }

        if (body.Private != null)
        {
            tag.Private = body.Private.Value;
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