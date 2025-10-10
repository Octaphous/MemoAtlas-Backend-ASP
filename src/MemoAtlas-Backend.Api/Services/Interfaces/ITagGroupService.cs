using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface ITagGroupService
{
    Task<IEnumerable<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsWithTagCountDataAsync(User user);
    Task<TagGroupWithTagsWithCountsDTO> GetTagGroupWithTagCountDataAsync(User user, int id);
    Task<TagGroup> GetTagGroupAsync(User user, int id);
    Task<TagGroup> CreateTagGroupAsync(User user, TagGroupCreateRequest body);
    Task<TagGroup> UpdateTagGroupAsync(User user, int id, TagGroupUpdateRequest body);
    Task DeleteTagGroupAsync(User user, int id);
}
