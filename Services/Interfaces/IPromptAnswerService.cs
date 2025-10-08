using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IPromptAnswerService
{
    void SetUpdatedPromptAnswers(Memo memo, IEnumerable<PromptAnswerUpdateRequest> updatedAnswers);
    Task<IEnumerable<PromptAnswer>> BuildPromptAnswersAsync(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers);
    (string?, double?) GetValidatedPromptValue(object value, Prompt prompt);
}
