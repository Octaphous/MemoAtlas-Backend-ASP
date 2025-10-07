using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class TagGroupService(AppDbContext db) : ITagGroupService
{
    public async Task<List<TagGroup>> GetAllTagGroupsAsync(User user)
    {
        List<TagGroup> tagGroups = await db.TagGroups
            .Where(tg => tg.UserId == user.Id)
            .Include(tg => tg.Tags)
            .ToListAsync();

        return tagGroups;
    }

    public async Task<TagGroup> GetTagGroupAsync(User user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        return tagGroup;
    }

    public async Task<TagGroup> CreateTagGroupAsync(User user, TagGroupCreateRequest body)
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

        return tagGroup;
    }

    public async Task<TagGroup> UpdateTagGroupAsync(User user, int id, TagGroupUpdateRequest body)
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
        return tagGroup;
    }

    public async Task DeleteTagGroupAsync(User user, int id)
    {
        TagGroup tagGroup = await db.TagGroups
            .Where(tg => tg.UserId == user.Id && tg.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Tag group not found.");

        db.TagGroups.Remove(tagGroup);
        await db.SaveChangesAsync();
    }

}