using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class PromptService(AppDbContext db) : IPromptService
{
    public async Task<List<Prompt>> GetAllPromptsAsync(User user)
    {
        List<Prompt> prompts = await db.Prompts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return prompts;
    }

    public async Task<List<Prompt>> GetPromptsAsync(User user, HashSet<int> promptIds)
    {
        if (promptIds.Count != promptIds.Distinct().Count())
        {
            throw new InvalidPayloadException("Some of the provided prompt IDs are duplicates.");
        }

        List<Prompt> prompts = await db.Prompts
            .Where(p => p.UserId == user.Id && promptIds.Contains(p.Id))
            .ToListAsync();

        if (prompts.Count != promptIds.Count)
        {
            throw new InvalidPayloadException("One or more of the provided prompt IDs do not exist for this user.");
        }

        return prompts;
    }

    public async Task<Prompt> GetPromptAsync(User user, int id)
    {
        Prompt prompt = await db.Prompts
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        return prompt;
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
        };

        db.Prompts.Add(prompt);
        await db.SaveChangesAsync();

        return prompt;
    }

    public async Task<Prompt> UpdatePromptAsync(User user, int id, PromptUpdateRequest body)
    {
        Prompt prompt = await db.Prompts
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        if (body.Question != null)
        {
            prompt.Question = body.Question;
        }

        db.Prompts.Update(prompt);
        await db.SaveChangesAsync();
        return prompt;
    }

    public async Task DeletePromptAsync(User user, int id)
    {
        Prompt prompt = await db.Prompts
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        db.Prompts.Remove(prompt);
        await db.SaveChangesAsync();
    }

    public async Task<List<PromptAnswer>> CreatePromptAnswersAsync(User user, List<PromptAnswerRequest> promptAnswers)
    {
        HashSet<int> promptIds = promptAnswers.Select(pv => pv.PromptId).ToHashSet();
        Dictionary<int, Prompt> promptDict = (await GetPromptsAsync(user, promptIds)).ToDictionary(p => p.Id);

        // Verify so that number prompts cant have text answers and vice versa
        foreach (PromptAnswerRequest pv in promptAnswers)
        {
            Prompt prompt = promptDict[pv.PromptId];

            if (prompt.Type == PromptType.Text && pv.Value is not string)
            {
                throw new InvalidPayloadException($"Prompt with id {prompt.Id} requires a text value.");
            }
            else if (prompt.Type == PromptType.Number && pv.Value is not double)
            {
                throw new InvalidPayloadException($"Prompt with id {prompt.Id} requires a number value.");
            }
        }

        return promptAnswers.Select(pv =>
        {
            Prompt prompt = promptDict[pv.PromptId];

            return new PromptAnswer
            {
                PromptId = prompt.Id,
                TextValue = prompt.Type == PromptType.Text ? (string)pv.Value : null,
                NumberValue = prompt.Type == PromptType.Number ? (double?)pv.Value : null
            };
        }).ToList();
    }
}