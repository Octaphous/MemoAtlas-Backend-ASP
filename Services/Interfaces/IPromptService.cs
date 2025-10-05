using MemoAtlas_Backend_ASP.Models.DTOs;

namespace MemoAtlas_Backend_ASP.Services.Interfaces
{
    public interface IPromptService
    {
        Task<List<PromptData>> GetAllPromptsAsync(UserData user);
        Task<PromptData> GetPromptAsync(UserData user, int id);
        Task<PromptData> CreatePromptAsync(UserData user, PromptCreateBody body);
        Task UpdatePromptAsync(UserData user, int id, PromptUpdateBody body);
        Task DeletePromptAsync(UserData user, int id);
    }
}