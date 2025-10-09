using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface ITagGroupRepository
{
    Task<List<TagGroup>> GetAllTagGroupsAsync(User user);
    Task<TagGroup?> GetTagGroupAsync(User user, int id);
    void AddTagGroup(TagGroup tagGroup);
    void DeleteTagGroup(TagGroup tagGroup);
    Task SaveChangesAsync();
}