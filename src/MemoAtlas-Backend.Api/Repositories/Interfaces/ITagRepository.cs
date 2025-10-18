using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagsAsync(User user);
    Task<List<Tag>> GetTagsAsync(User user, HashSet<int> tagIds);
    Task<Tag?> GetTagAsync(User user, int id);
    Task<List<Tag>> GetTagsBySearchStringAsync(User user, string query);

    void AddTag(Tag tag);
    void DeleteTag(Tag tag);
    Task SaveChangesAsync();
}