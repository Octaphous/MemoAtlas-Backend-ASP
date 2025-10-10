using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface ITagGroupRepository
{
    IQueryable<TagGroupWithTagsWithCountsDTO> TagGroupWithTagCountData(User user, int? tagGroupId = null);
    Task<List<TagGroupWithTagsWithCountsDTO>> GetAllTagGroupsWithTagCountDataAsync(User user);
    Task<TagGroupWithTagsWithCountsDTO?> GetTagGroupWithTagCountDataAsync(User user, int id);
    Task<TagGroup?> GetTagGroupAsync(User user, int id);
    void AddTagGroup(TagGroup tagGroup);
    void DeleteTagGroup(TagGroup tagGroup);
    Task SaveChangesAsync();
}