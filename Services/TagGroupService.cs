using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagGroupService(AppDbContext db) : ITagGroupService
{
    public async Task<List<TagGroupResponse>> GetAllTagGroupsAsync(UserResponse user)
    {
        List<TagGroup> tagGroups = await db.TagGroups
            .Where(tg => tg.UserId == user.Id)
            .Include(tg => tg.Tags)
            .ToListAsync();

        return [.. tagGroups.Select(tg => new TagGroupResponse(tg))];
    }

    public async Task<TagGroupResponse> GetTagGroupAsync(UserResponse user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        return new TagGroupResponse(tagGroup);
    }

    public async Task<TagGroupResponse> CreateTagGroupAsync(UserResponse user, TagGroupCreateRequest body)
    {
        if (!AppConstants.AllowedTagColors.Contains(body.Color))
        {
            throw new InvalidPayloadException("Specified color is not allowed.");
        }

        TagGroup tagGroup = new()
        {
            UserId = user.Id,
            Name = body.Name,
            Color = body.Color
        };

        db.TagGroups.Add(tagGroup);
        await db.SaveChangesAsync();

        return new TagGroupResponse(tagGroup);
    }

    public async Task UpdateTagGroupAsync(UserResponse user, int id, TagGroupUpdateRequest body)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        if (body.Name != null)
        {
            tagGroup.Name = body.Name;
        }

        if (body.Color != null)
        {
            if (!AppConstants.AllowedTagColors.Contains(body.Color))
            {
                throw new InvalidPayloadException("Specified color is not allowed.");
            }
            tagGroup.Color = body.Color.ToLower();
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteTagGroupAsync(UserResponse user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        db.TagGroups.Remove(tagGroup);
        await db.SaveChangesAsync();
    }

}