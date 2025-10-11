using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IPromptAnswerService
{
    void SetUpdatedPromptAnswers(Memo memo, IEnumerable<PromptAnswerUpdateRequest> updatedAnswers);
    Task<IEnumerable<PromptAnswer>> BuildPromptAnswersAsync(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers);
}
