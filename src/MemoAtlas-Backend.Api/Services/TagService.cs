using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class TagService(ITagRepository tagRepository, ITagGroupService tagGroupService) : ITagService
{
    public async Task<IEnumerable<Tag>> GetAllTagsAsync(User user)
    {
        IEnumerable<Tag> tags = await tagRepository.GetAllTagsAsync(user);

        tags = tags.Where(tag => TagVisibleToUser(tag, user));

        return tags;
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync(User user, HashSet<int> tagIds)
    {
        if (tagIds.Count != tagIds.Distinct().Count())
        {
            throw new InvalidPayloadException("Some of the provided tag IDs are duplicates.");
        }

        List<Tag> tags = (await tagRepository.GetTagsAsync(user, tagIds)).ToList();

        if (tags.Count != tagIds.Count)
        {
            throw new InvalidPayloadException("One or more of the provided tag IDs do not exist for this user.");
        }

        tags = tags.Where(tag => TagVisibleToUser(tag, user)).ToList();

        return tags;
    }

    public async Task<Tag> GetTagAsync(User user, int id)
    {
        Tag? tag = await tagRepository.GetTagAsync(user, id);

        if (tag == null || TagVisibleToUser(tag, user) == false)
        {
            throw new InvalidResourceException("Tag not found.");
        }

        tag.Memos = tag.Memos.Where(m => MemoVisibleToUser(m, user)).ToList();

        return tag;
    }

    public async Task<Tag> CreateTagAsync(User user, TagCreateRequest body)
    {
        TagGroup tagGroup = await tagGroupService.GetTagGroupAsync(user, body.GroupId);

        Tag tag = new()
        {
            TagGroupId = body.GroupId,
            Name = body.Name,
            Description = body.Description,
            Private = body.Private
        };

        tagRepository.AddTag(tag);
        await tagRepository.SaveChangesAsync();
        tag.TagGroup = tagGroup;

        return tag;
    }

    public async Task<Tag> UpdateTagAsync(User user, int id, TagUpdateRequest body)
    {
        Tag? tag = await tagRepository.GetTagAsync(user, id);

        if (tag == null || TagVisibleToUser(tag, user) == false)
        {
            throw new InvalidResourceException("Tag not found.");
        }

        tag.Memos = tag.Memos.Where(m => MemoVisibleToUser(m, user)).ToList();

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

        await tagRepository.SaveChangesAsync();
        return tag;
    }

    public async Task DeleteTagAsync(User user, int id)
    {
        Tag tag = await GetTagAsync(user, id);
        tagRepository.DeleteTag(tag);
        await tagRepository.SaveChangesAsync();
    }

    static bool TagVisibleToUser(Tag tag, User user)
    {
        return user.PrivateMode || !tag.Private && !tag.TagGroup.Private;
    }

    static bool MemoVisibleToUser(Memo memo, User user)
    {
        return user.PrivateMode || !memo.Private;
    }
}