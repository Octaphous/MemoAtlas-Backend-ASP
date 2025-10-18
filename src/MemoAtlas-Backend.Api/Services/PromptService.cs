using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class PromptService(IPromptRepository promptRepository) : IPromptService
{
    public async Task<IEnumerable<Prompt>> GetAllPromptsAsync(User user)
    {
        IEnumerable<Prompt> prompts = await promptRepository.GetAllPromptsAsync(user);

        return prompts;
    }

    public async Task<IEnumerable<Prompt>> GetPromptsAsync(User user, HashSet<int> promptIds)
    {
        if (promptIds.Count != promptIds.Distinct().Count())
        {
            throw new InvalidPayloadException("Some of the provided prompt IDs are duplicates.");
        }

        List<Prompt> prompts = await promptRepository.GetPromptsAsync(user, promptIds);

        if (prompts.Count != promptIds.Count)
        {
            throw new InvalidPayloadException("One or more of the provided prompt IDs do not exist for this user.");
        }

        return prompts;
    }

    public async Task<Prompt> GetPromptAsync(User user, int id)
    {
        Prompt prompt = await promptRepository.GetPromptAsync(user, id)
            ?? throw new InvalidResourceException("Prompt not found.");

        return prompt;
    }

    public async Task<List<Prompt>> SearchPromptsAsync(User user, string query)
    {
        return await promptRepository.GetPromptsBySearchStringAsync(user, query);
    }

    public async Task<Prompt> CreatePromptAsync(User user, PromptCreateRequest body)
    {
        if (body.Type < 0 || body.Type > 2)
        {
            throw new InvalidPayloadException("Invalid prompt type.");
        }

        Prompt prompt = new()
        {
            UserId = user.Id,
            Question = body.Question,
            Type = (PromptType)body.Type,
            Private = body.Private
        };

        promptRepository.AddPrompt(prompt);
        await promptRepository.SaveChangesAsync();

        return await GetPromptAsync(user, prompt.Id);
    }

    public async Task<Prompt> UpdatePromptAsync(User user, int id, PromptUpdateRequest body)
    {
        Prompt prompt = await GetPromptAsync(user, id);

        if (body.Question != null)
        {
            prompt.Question = body.Question;
        }

        if (body.Private != null)
        {
            prompt.Private = body.Private.Value;
        }

        await promptRepository.SaveChangesAsync();
        return await GetPromptAsync(user, id);
    }

    public async Task DeletePromptAsync(User user, int id)
    {
        Prompt prompt = await GetPromptAsync(user, id);

        promptRepository.DeletePrompt(prompt);
        await promptRepository.SaveChangesAsync();
    }
}