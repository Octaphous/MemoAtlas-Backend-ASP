using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync(User user);
    Task<List<Tag>> GetTagsAsync(User user, HashSet<int> tagIds);
    Task<Tag> GetTagAsync(User user, int id);
    Task<Tag> CreateTagAsync(User user, TagCreateRequest body);
    Task<Tag> UpdateTagAsync(User user, int id, TagUpdateRequest body);
    Task DeleteTagAsync(User user, int id);
}
