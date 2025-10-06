using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IPromptService
{
    Task<List<PromptResponse>> GetAllPromptsAsync(UserResponse user);
    Task<List<PromptResponse>> GetPromptsAsync(UserResponse user, List<int> promptIds);
    Task<PromptResponse> GetPromptAsync(UserResponse user, int id);
    Task<PromptResponse> CreatePromptAsync(UserResponse user, PromptCreateRequest body);
    Task UpdatePromptAsync(UserResponse user, int id, PromptUpdateRequest body);
    Task DeletePromptAsync(UserResponse user, int id);
}
