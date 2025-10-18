using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface ITagGroupService
{
    Task<IEnumerable<TagGroup>> GetAllTagGroupsAsync(User user);
    Task<TagGroup> GetTagGroupAsync(User user, int id);
    Task<IEnumerable<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsStatsAsync(User user, TagGroupStatsFilterRequest filter);
    Task<List<TagGroup>> SearchTagGroupsAsync(User user, string query);
    Task<TagGroup> CreateTagGroupAsync(User user, TagGroupCreateRequest body);
    Task<TagGroup> UpdateTagGroupAsync(User user, int id, TagGroupUpdateRequest body);
    Task DeleteTagGroupAsync(User user, int id);
}
