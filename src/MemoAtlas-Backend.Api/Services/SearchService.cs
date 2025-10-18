using MemoAtlas_Backend.Api.Mappers;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class SearchService(ITagService tagService, IMemoService memoService, ITagGroupService tagGroupService, IPromptService promptService) : ISearchService
{
    public async Task<SearchResultsDTO> SearchAll(User user, string query)
    {
        List<Memo> memos = await memoService.SearchMemosAsync(user, query);
        List<Tag> tags = await tagService.SearchTagsAsync(user, query);
        List<TagGroup> tagGroups = await tagGroupService.SearchTagGroupsAsync(user, query);
        List<Prompt> prompts = await promptService.SearchPromptsAsync(user, query);

        return new SearchResultsDTO
        {
            Memos = memos.Select(MemoMapper.ToDTO),
            Tags = tags.Select(TagMapper.ToTagWithGroupDTO),
            TagGroups = tagGroups.Select(TagGroupMapper.ToDTO),
            Prompts = prompts.Select(PromptMapper.ToDTO)
        };
    }
}