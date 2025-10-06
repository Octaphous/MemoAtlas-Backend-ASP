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
    public async Task<List<PromptResponse>> GetAllPromptsAsync(UserResponse user)
    {
        List<Prompt> prompts = await db.Prompts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return [.. prompts.Select(p => new PromptResponse(p))];
    }

    public async Task<List<PromptResponse>> GetPromptsAsync(UserResponse user, List<int> promptIds)
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

        return [.. prompts.Select(p => new PromptResponse(p))];
    }

    public async Task<PromptResponse> GetPromptAsync(UserResponse user, int id)
    {
        Prompt prompt = await db.Prompts
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        return new PromptResponse(prompt);
    }

    public async Task<PromptResponse> CreatePromptAsync(UserResponse user, PromptCreateRequest body)
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

        return new PromptResponse(prompt);
    }

    public async Task UpdatePromptAsync(UserResponse user, int id, PromptUpdateRequest body)
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
    }

    public async Task DeletePromptAsync(UserResponse user, int id)
    {
        Prompt prompt = await db.Prompts
            .Where(p => p.UserId == user.Id && p.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException("Prompt not found.");

        db.Prompts.Remove(prompt);
        await db.SaveChangesAsync();
    }
}