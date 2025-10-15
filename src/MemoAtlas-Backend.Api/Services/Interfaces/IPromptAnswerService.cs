using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IPromptAnswerService
{
    void SetUpdatedPromptAnswers(List<PromptAnswer> existingAnswers, IEnumerable<PromptAnswerUpdateRequest> updatedAnswers);
    Task ValidatePromptAnswerRequests(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers);
    Task<IEnumerable<PromptAnswer>> BuildPromptAnswersAsync(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers);
}
