using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagGroupService(AppDbContext db) : ITagGroupService
{
    public async Task<List<TagGroupData>> GetAllTagGroupsAsync(UserData user)
    {
        List<TagGroup> tagGroups = await db.TagGroups
            .Where(tg => tg.UserId == user.Id)
            .Include(tg => tg.Tags)
            .ToListAsync();

        return [.. tagGroups.Select(tg => new TagGroupData(tg))];
    }

    public async Task<TagGroupData> GetTagGroupAsync(UserData user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        return new TagGroupData(tagGroup);
    }

    public async Task<TagGroupData> CreateTagGroupAsync(UserData user, TagGroupCreateBody body)
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

        return new TagGroupData(tagGroup);
    }

    public async Task UpdateTagGroupAsync(UserData user, int id, TagGroupUpdateBody body)
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

    public async Task DeleteTagGroupAsync(UserData user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        db.TagGroups.Remove(tagGroup);
        await db.SaveChangesAsync();
    }

}