using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IPromptStatsService
{
    Task<PromptStatsAllDTO> GetAllPromptStatsAsync(User user, PromptStatsFilterRequest filter);
    Task<List<PromptStatsNumbersDTO>> GetAllNumberPromptStatsAsync(User user, PromptStatsFilterRequest filter);
}