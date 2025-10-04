using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class MemoService(AppDbContext db, IUserContext userContext) : IMemoService
{
    public async Task<List<MemoData>> GetAllMemosAsync()
    {
        return [];
    }

    public async Task<MemoData> CreateMemoAsync(MemoCreateBody body)
    {
        UserData user = userContext.GetRequiredUser();

        bool dateExists = await db.Memos.AnyAsync(m => m.UserId == user.Id && m.Date == body.Date);
        if (dateExists)
        {
            throw new InvalidOperationException("A memo for this date already exists.");
        }

        List<int> tagIds = body.Tags ?? [];
        if (tagIds.Count != tagIds.Distinct().Count())
        {
            throw new InvalidOperationException("Duplicate tags are not allowed.");
        }

        List<Tag> tags = await db.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
        if (tags.Count != tagIds.Count)
        {
            throw new InvalidOperationException("One or more tags do not exist.");
        }

        List<PromptValueBody> promptValues = body.Prompts ?? [];
        List<int> promptIds = [.. promptValues.Select(pv => pv.PromptId)];
        if (promptIds.Count != promptIds.Distinct().Count())
        {
            throw new InvalidOperationException("Duplicate prompts are not allowed.");
        }

        List<Prompt> prompts = await db.Prompts.Where(p => p.UserId == user.Id && promptIds.Contains(p.Id)).ToListAsync();
        if (prompts.Count != promptIds.Count)
        {
            throw new InvalidOperationException("One or more prompts do not exist.");
        }

        foreach (PromptValueBody pv in promptValues)
        {
            Prompt prompt = prompts.First(p => p.Id == pv.PromptId);

            if (prompt.Type == PromptType.Text && pv.Value is not string)
            {
                throw new InvalidOperationException($"Prompt {prompt.Id} requires a text value.");
            }
            else if (prompt.Type == PromptType.Number && pv.Value is not int)
            {
                throw new InvalidOperationException($"Prompt {prompt.Id} requires a number value.");
            }
        }

        List<PromptAnswer> promptAnswers = [.. promptValues.Select(pv =>
        {
            Prompt prompt = prompts.First(p => p.Id == pv.PromptId);

            return new PromptAnswer
            {
                PromptId = prompt.Id,
                TextValue = prompt.Type == PromptType.Text ? (string)pv.Value : null,
                NumberValue = prompt.Type == PromptType.Number ? (int?)pv.Value : null
            };
        })];

        Memo memo = new()
        {
            UserId = user.Id,
            Date = body.Date,
            Tags = tags,
            PromptAnswers = promptAnswers
        };

        db.Memos.Add(memo);
        await db.SaveChangesAsync();

        return new MemoData(memo);
    }
}