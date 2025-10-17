using System.Diagnostics;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class PromptAnswerService(IPromptService promptService) : IPromptAnswerService
{
    public void SetUpdatedPromptAnswers(List<PromptAnswer> existingAnswers, IEnumerable<PromptAnswerUpdateRequest> updatedAnswers)
    {
        HashSet<int> updatedIds = updatedAnswers.Select(pa => pa.Id).ToHashSet();
        HashSet<int> existingIds = existingAnswers.Select(pa => pa.Id).ToHashSet();

        if (updatedIds.Except(existingIds).Any())
        {
            throw new InvalidPayloadException("One or more of the provided PromptAnswer IDs do not exist in the existing answers.");
        }

        foreach (PromptAnswerUpdateRequest update in updatedAnswers)
        {
            PromptAnswer existing = existingAnswers.First(pa => pa.Id == update.Id);

            if (update.Private != null)
            {
                existing.Private = update.Private.Value;
            }

            switch (existing)
            {
                case PromptAnswerText textAnswer when update is PromptAnswerTextUpdateRequest textUpdate:
                    if (textUpdate.Text != null)
                    {
                        textAnswer.Text = textUpdate.Text;
                    }
                    break;

                case PromptAnswerNumber numberAnswer when update is PromptAnswerNumberUpdateRequest numberUpdate:
                    if (numberUpdate.Number != null)
                    {
                        numberAnswer.Number = numberUpdate.Number.Value;
                    }
                    break;

                default:
                    throw new InvalidPayloadException($"Mismatched PromptAnswer and UpdateRequest types for PromptAnswer ID {existing.Id}.");
            }
        }
    }

    // Verify that provided prompt answer value matches the prompt type, so that we for example don't try to store a text answer for a number prompt
    public async Task ValidatePromptAnswerRequestsAsync(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers)
    {
        HashSet<int> answerPromptIds = promptAnswers.Select(pa => pa.PromptId).ToHashSet();
        Dictionary<int, Prompt> prompts = (await promptService.GetPromptsAsync(user, answerPromptIds)).ToDictionary(p => p.Id);

        foreach (PromptAnswerCreateRequest answer in promptAnswers)
        {
            Prompt prompt = prompts[answer.PromptId] ?? throw new InvalidResourceException($"Prompt with ID {answer.PromptId} not found.");

            if (prompt.Type != answer.Type)
            {
                throw new InvalidPayloadException($"Prompt with ID {answer.PromptId} is of type {prompt.Type}, but received answer of type {answer.Type}.");
            }
        }
    }

    // Construct PromptAnswer database entities from request DTOs 
    public IEnumerable<PromptAnswer> BuildPromptAnswers(User user, IEnumerable<PromptAnswerCreateRequest> promptAnswers)
    {
        List<PromptAnswer> builtAnswers = [];

        foreach (PromptAnswerCreateRequest pa in promptAnswers)
        {
            switch (pa)
            {
                case PromptAnswerTextCreateRequest req:
                    builtAnswers.Add(new PromptAnswerText
                    {
                        PromptId = req.PromptId,
                        Text = req.Text,
                        Private = req.Private
                    });
                    break;

                case PromptAnswerNumberCreateRequest req:
                    builtAnswers.Add(new PromptAnswerNumber
                    {
                        PromptId = req.PromptId,
                        Number = req.Number,
                        Private = req.Private
                    });
                    break;

                default:
                    throw new UnreachableException("Unknown PromptAnswerCreateRequest type.");
            }
        }

        return builtAnswers;
    }
}