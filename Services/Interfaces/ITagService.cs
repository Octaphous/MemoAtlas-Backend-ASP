using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ITagService
{
    Task<List<TagData>> GetAllTagsAsync(UserData user);
    Task<List<TagData>> GetTagsAsync(UserData user, List<int> tagIds);
    Task<TagData> GetTagAsync(UserData user, int id);
    Task<TagData> CreateTagAsync(UserData user, TagCreateBody body);
    Task UpdateTagAsync(UserData user, int id, TagUpdateBody body);
    Task DeleteTagAsync(UserData user, int id);
}
