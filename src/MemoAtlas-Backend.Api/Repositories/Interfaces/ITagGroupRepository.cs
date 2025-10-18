using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface ITagGroupRepository
{
    Task<List<TagGroup>> GetAllTagGroupsAsync(User user);
    Task<TagGroup?> GetTagGroupAsync(User user, int id);
    Task<List<TagGroup>> GetTagGroupsBySearchStringAsync(User user, string query);
    Task<List<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsStatsAsync(User user, TagGroupStatsFilterRequest filter);

    void AddTagGroup(TagGroup tagGroup);
    void DeleteTagGroup(TagGroup tagGroup);
    Task SaveChangesAsync();
}