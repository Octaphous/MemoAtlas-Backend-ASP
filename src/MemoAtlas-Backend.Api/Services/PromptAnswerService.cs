using System.Diagnostics;
using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class PromptAnswerService(IPromptService promptService) : IPromptAnswerService
{
    public void SetUpdatedPromptAnswers(Memo memo, IEnumerable<PromptAnswerUpdateRequest> updatedAnswers)
    {
        HashSet<int> updatedAnswersIds = updatedAnswers
            .Select(pa => pa.Id)
            .ToHashSet();

        HashSet<int> existingAnswersIds = memo.PromptAnswers
            .Select(pa => pa.Id)
            .ToHashSet();

        if (updatedAnswersIds.Except(existingAnswersIds).Any())
        {
            throw new InvalidOperationException("One or more of the provided prompt answer IDs do not exist for this memo.");
        }

        Dictionary<int, Prompt> prompts = memo.PromptAnswers
            .Where(pa => updatedAnswersIds
                .Contains(pa.Id))
            .Select(pa => pa.Prompt)
            .ToDictionary(p => p.Id);

        Dictionary<int, PromptAnswer> existingAnswers = memo.PromptAnswers
            .Where(pa => updatedAnswersIds
                .Contains(pa.Id))
            .ToDictionary(pa => pa.Id);

        foreach (PromptAnswerUpdateRequest update in updatedAnswers)
        {
            PromptAnswer existing = existingAnswers[update.Id];

            if (update.Value != null)
            {
                Prompt prompt = prompts[existing.PromptId];
                (string? textValue, double? numberValue) = GetValidatedPromptValue(update.Value, prompt);
                existing.TextValue = textValue;
                existing.NumberValue = numberValue;
            }

            if (update.Private != null)
            {
                existing.Private = update.Private.Value;
            }
        }
    }

    public async Task<IEnumerable<PromptAnswer>> BuildPromptAnswersAsync(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers)
    {
        HashSet<int> promptIds = promptAnswers
            .Select(pa => pa.PromptId)
            .ToHashSet();

        Dictionary<int, Prompt> prompts = (await promptService
            .GetPromptsAsync(user, promptIds))
            .ToDictionary(p => p.Id);

        List<PromptAnswer> builtAnswers = [];

        foreach (PromptAnswerCreateRequest pa in promptAnswers)
        {
            Prompt prompt = prompts[pa.PromptId]
                ?? throw new InvalidOperationException($"Prompt with id {pa.PromptId} does not exist.");

            (string? textValue, double? numberValue) = GetValidatedPromptValue(pa.Value, prompt);

            builtAnswers.Add(new PromptAnswer
            {
                PromptId = prompt.Id,
                TextValue = textValue,
                NumberValue = numberValue,
                Private = pa.Private
            });
        }

        return builtAnswers;
    }

    public (string?, double?) GetValidatedPromptValue(object value, Prompt prompt)
    {
        return prompt.Type switch
        {
            PromptType.Text when value is string text => (text, null),

            PromptType.Number when double.TryParse(value?.ToString(), out var number)
                => (null, number),

            PromptType.Text => throw new InvalidOperationException($"Prompt with ID {prompt.Id} requires a text value."),
            PromptType.Number => throw new InvalidOperationException($"Prompt with ID {prompt.Id} requires a numeric value."),
            _ => throw new UnreachableException($"Unsupported prompt type for prompt with ID {prompt.Id}.")
        };
    }
}