using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;

public class PromptStatsService(IPromptStatsRepository promptStatsRepository) : IPromptStatsService
{
    public async Task<PromptStatsAllDTO> GetAllPromptStatsAsync(User user, PromptStatsFilterRequest filter)
    {
        return new PromptStatsAllDTO
        {
            NumberPrompts = await GetAllNumberPromptStatsAsync(user, filter)
        };
    }

    public async Task<List<PromptStatsNumbersDTO>> GetAllNumberPromptStatsAsync(User user, PromptStatsFilterRequest filter)
    {
        Validators.ValidateOptionalDateSpan(filter.StartDate, filter.EndDate);
        return await promptStatsRepository.GetAllNumberAnswerStatsAsync(user, filter);
    }
}