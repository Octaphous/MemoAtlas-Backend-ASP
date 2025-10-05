using MemoAtlas_Backend_ASP.Models.DTOs;

namespace MemoAtlas_Backend_ASP.Services.Interfaces
{
    public interface ITagGroupService
    {
        Task<List<TagGroupData>> GetAllTagGroupsAsync(UserData user);
        Task<TagGroupData> GetTagGroupAsync(UserData user, int id);
        Task<TagGroupData> CreateTagGroupAsync(UserData user, TagGroupCreateBody body);
        Task UpdateTagGroupAsync(UserData user, int id, TagGroupUpdateBody body);
        Task DeleteTagGroupAsync(UserData user, int id);
    }
}