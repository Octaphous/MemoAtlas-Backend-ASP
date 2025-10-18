using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface IPromptRepository
{
    Task<List<Prompt>> GetAllPromptsAsync(User user);
    Task<List<Prompt>> GetPromptsAsync(User user, HashSet<int> promptIds);
    Task<Prompt?> GetPromptAsync(User user, int id);
    Task<List<Prompt>> GetPromptsBySearchStringAsync(User user, string query);

    void AddPrompt(Prompt prompt);
    void DeletePrompt(Prompt prompt);
    Task SaveChangesAsync();
}