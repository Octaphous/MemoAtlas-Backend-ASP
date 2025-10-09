using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class TagGroupService(ITagGroupRepository tagGroupRepository) : ITagGroupService
{
    public async Task<IEnumerable<TagGroup>> GetAllTagGroupsAsync(User user)
    {
        List<TagGroup> tagGroups = await tagGroupRepository.GetAllTagGroupsAsync(user);

        if (!user.PrivateMode)
        {
            foreach (TagGroup tg in tagGroups)
            {
                tg.Tags = tg.Tags.Where(tag => !tag.Private).ToList();
            }
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
            Color = body.Color,
            Private = body.Private
        };

        tagGroupRepository.AddTagGroup(tagGroup);
        await tagGroupRepository.SaveChangesAsync();

        return tagGroup;
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
            if (!AppConstants.AllowedTagColors.Contains(body.Color))
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
        return tagGroup;
    }

    public async Task DeleteTagGroupAsync(User user, int id)
    {
        TagGroup tagGroup = await GetTagGroupAsync(user, id);

        tagGroupRepository.DeleteTagGroup(tagGroup);
        await tagGroupRepository.SaveChangesAsync();
    }
}