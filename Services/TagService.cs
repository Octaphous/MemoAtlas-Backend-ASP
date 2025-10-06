using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagService(AppDbContext db) : ITagService
{
    public async Task<List<TagDetailedResponse>> GetAllTagsAsync(UserResponse user)
    {
        List<Tag> tags = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id)
            .Include(t => t.TagGroup)
            .ToListAsync();

        return [.. tags.Select(t => new TagDetailedResponse(t))];
    }

    public async Task<List<TagDetailedResponse>> GetTagsAsync(UserResponse user, List<int> tagIds)
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

        return [.. tags.Select(t => new TagDetailedResponse(t))];
    }

    public async Task<TagDetailedResponse> GetTagAsync(UserResponse user, int id)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .Include(t => t.TagGroup)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        return new TagDetailedResponse(tag);
    }

    public async Task<TagDetailedResponse> CreateTagAsync(UserResponse user, TagCreateRequest body)
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

        return new TagDetailedResponse(tag);
    }

    public async Task UpdateTagAsync(UserResponse user, int id, TagUpdateRequest body)
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

    public async Task DeleteTagAsync(UserResponse user, int id)
    {
        Tag tag = await db.Tags
            .Where(t => t.TagGroup.UserId == user.Id && t.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag not found.");

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
    }
}