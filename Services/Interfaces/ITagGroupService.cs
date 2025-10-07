using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ITagGroupService
{
    Task<List<TagGroup>> GetAllTagGroupsAsync(User user);
    Task<TagGroup> GetTagGroupAsync(User user, int id);
    Task<TagGroup> CreateTagGroupAsync(User user, TagGroupCreateRequest body);
    Task<TagGroup> UpdateTagGroupAsync(User user, int id, TagGroupUpdateRequest body);
    Task DeleteTagGroupAsync(User user, int id);
}
