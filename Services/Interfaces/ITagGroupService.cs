using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ITagGroupService
{
    Task<List<TagGroupResponse>> GetAllTagGroupsAsync(UserResponse user);
    Task<TagGroupResponse> GetTagGroupAsync(UserResponse user, int id);
    Task<TagGroupResponse> CreateTagGroupAsync(UserResponse user, TagGroupCreateRequest body);
    Task UpdateTagGroupAsync(UserResponse user, int id, TagGroupUpdateRequest body);
    Task DeleteTagGroupAsync(UserResponse user, int id);
}
