using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IPromptService
{
    Task<List<Prompt>> GetAllPromptsAsync(User user);
    Task<List<Prompt>> GetPromptsAsync(User user, HashSet<int> promptIds);
    Task<Prompt> GetPromptAsync(User user, int id);
    Task<Prompt> CreatePromptAsync(User user, PromptCreateRequest body);
    Task<Prompt> UpdatePromptAsync(User user, int id, PromptUpdateRequest body);
    Task DeletePromptAsync(User user, int id);
}
