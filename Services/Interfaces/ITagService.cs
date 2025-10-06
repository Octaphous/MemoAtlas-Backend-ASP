using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ITagService
{
    Task<List<TagDetailedResponse>> GetAllTagsAsync(UserResponse user);
    Task<List<TagDetailedResponse>> GetTagsAsync(UserResponse user, List<int> tagIds);
    Task<TagDetailedResponse> GetTagAsync(UserResponse user, int id);
    Task<TagDetailedResponse> CreateTagAsync(UserResponse user, TagCreateRequest body);
    Task UpdateTagAsync(UserResponse user, int id, TagUpdateRequest body);
    Task DeleteTagAsync(UserResponse user, int id);
}
