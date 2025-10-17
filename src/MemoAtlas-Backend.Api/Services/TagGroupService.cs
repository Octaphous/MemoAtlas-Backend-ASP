using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;

namespace MemoAtlas_Backend.Api.Services;

public class TagGroupService(ITagGroupRepository tagGroupRepository) : ITagGroupService
{
    private static readonly HashSet<string> AllowedColors = new(StringComparer.OrdinalIgnoreCase)
        {
            "red", "blue", "green", "yellow"
        };

    public async Task<IEnumerable<TagGroup>> GetAllTagGroupsAsync(User user)
    {
        List<TagGroup> tagGroups = await tagGroupRepository.GetAllTagGroupsAsync(user);

        foreach (TagGroup tg in tagGroups)
        {
            tg.Tags = user.PrivateMode ? tg.Tags : tg.Tags.Where(tag => !tag.Private).ToList();
        }

        return tagGroups;
    }

    public async Task<TagGroup> GetTagGroupAsync(User user, int id)
    {
        TagGroup tagGroup = await tagGroupRepository.GetTagGroupAsync(user, id)
            ?? throw new InvalidResourceException("Tag group not found.");

        tagGroup.Tags = tagGroup.Tags.Where(tag => !tag.Private || user.PrivateMode).ToList();

        return tagGroup;
    }

    public async Task<IEnumerable<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsStatsAsync(User user, TagGroupStatsFilterRequest filter)
    {
        Validators.ValidateOptionalDateSpan(filter.StartDate, filter.EndDate);

        IEnumerable<TagGroupWithTagsWithCountsDTO> tagGroups =
            await tagGroupRepository.GetAllTagGroupsStatsAsync(user, filter);

        foreach (TagGroupWithTagsWithCountsDTO tg in tagGroups)
        {
            tg.Tags = user.PrivateMode ? tg.Tags : tg.Tags.Where(tag => !tag.Private);
        }

        return tagGroups;
    }

    public async Task<TagGroup> CreateTagGroupAsync(User user, TagGroupCreateRequest body)
    {
        if (!AllowedColors.Contains(body.Color))
        {
            throw new InvalidPayloadException("Specified color is not allowed.");
        }

        TagGroup tagGroup = new()
        {
            UserId = user.Id,
            Name = body.Name,
            Color = body.Color,
            Private = body.Private
        };

        tagGroupRepository.AddTagGroup(tagGroup);
        await tagGroupRepository.SaveChangesAsync();

        return await GetTagGroupAsync(user, tagGroup.Id);
    }

    public async Task<TagGroup> UpdateTagGroupAsync(User user, int id, TagGroupUpdateRequest body)
    {
        TagGroup tagGroup = await GetTagGroupAsync(user, id);

        if (body.Name != null)
        {
            tagGroup.Name = body.Name;
        }

        if (body.Color != null)
        {
            if (!AllowedColors.Contains(body.Color))
            {
                throw new InvalidPayloadException("Specified color is not allowed.");
            }
            tagGroup.Color = body.Color.ToLower();
        }

        if (body.Private != null)
        {
            tagGroup.Private = body.Private.Value;
        }

        await tagGroupRepository.SaveChangesAsync();
        return await GetTagGroupAsync(user, tagGroup.Id);
    }

    public async Task DeleteTagGroupAsync(User user, int id)
    {
        TagGroup tagGroup = await GetTagGroupAsync(user, id);

        tagGroupRepository.DeleteTagGroup(tagGroup);
        await tagGroupRepository.SaveChangesAsync();
    }
}