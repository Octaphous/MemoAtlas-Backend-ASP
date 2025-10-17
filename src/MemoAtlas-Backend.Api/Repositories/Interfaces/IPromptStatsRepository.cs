using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface IPromptStatsRepository
{
    Task<List<PromptStatsNumbersDTO>> GetAllNumberAnswerStatsAsync(User user, PromptStatsFilterRequest filter);
}