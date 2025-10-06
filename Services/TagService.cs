using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagService(AppDbContext db) : ITagService
{
    public async Task<List<TagData>> GetAllTagsAsync(UserData user)
    {
        List<Tag> tags = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id)
            .Include(t => t.TagGroup)
            .ToListAsync();

        return [.. tags.Select(t => new TagData(t))];
    }

    public async Task<List<TagData>> GetTagsAsync(UserData user, List<int> tagIds)
    {
        List<Tag> tags = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && tagIds.Contains(t.Id))
            .Include(t => t.TagGroup)
            .ToListAsync();

        return [.. tags.Select(t => new TagData(t))];
    }

    public async Task<TagData> GetTagAsync(UserData user, int id)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        return new TagData(tag);
    }

    public async Task<TagData> CreateTagAsync(UserData user, TagCreateBody body)
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

        return new TagData(tag);
    }

    public async Task UpdateTagAsync(UserData user, int id, TagUpdateBody body)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
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
    }

    public async Task DeleteTagAsync(UserData user, int id)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
    }
}