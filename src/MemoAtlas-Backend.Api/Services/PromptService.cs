using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Services;

public class PromptService(AppDbContext db) : IPromptService
{
    public async Task<List<Prompt>> GetAllPromptsAsync(User user)
    {
        List<Prompt> prompts = await db.Prompts
            .VisibleToUser(user)
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
            .VisibleToUser(user)
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
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && p.Id == id)
            .Include(p => p.PromptAnswers)
            .ThenInclude(pa => pa.Memo)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        // If user is not in private mode, remove any prompt answers that are private or belong to a private memo
        prompt.PromptAnswers = prompt.PromptAnswers.Where(pa => (!pa.Private && !pa.Memo.Private) || user.PrivateMode).ToList();

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
            Private = body.Private
        };

        db.Prompts.Add(prompt);
        await db.SaveChangesAsync();

        return prompt;
    }

    public async Task<Prompt> UpdatePromptAsync(User user, int id, PromptUpdateRequest body)
    {
        Prompt prompt = await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        if (body.Question != null)
        {
            prompt.Question = body.Question;
        }

        if (body.Private != null)
        {
            prompt.Private = body.Private.Value;
        }

        await db.SaveChangesAsync();
        return prompt;
    }

    public async Task DeletePromptAsync(User user, int id)
    {
        Prompt prompt = await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        db.Prompts.Remove(prompt);
        await db.SaveChangesAsync();
    }
}