using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class PromptRepository(AppDbContext db) : IPromptRepository
{
    public async Task<List<Prompt>> GetAllPromptsAsync(User user)
    {
        return await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id)
            .ToListAsync();
    }

    public async Task<List<Prompt>> GetPromptsAsync(User user, HashSet<int> promptIds)
    {
        return await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && promptIds.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<Prompt?> GetPromptAsync(User user, int id)
    {
        return await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && p.Id == id)
            .Include(p => p.PromptAnswers)
            .ThenInclude(pa => pa.Memo)
            .FirstOrDefaultAsync();
    }

    public Task<List<Prompt>> GetPromptsBySearchAsync(User user, string query)
    {
        return db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id && EF.Functions.Like(p.Question, $"%{query}%"))
            .ToListAsync();
    }

    public void AddPrompt(Prompt prompt)
    {
        db.Prompts.Add(prompt);
    }

    public void DeletePrompt(Prompt prompt)
    {
        db.Prompts.Remove(prompt);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}