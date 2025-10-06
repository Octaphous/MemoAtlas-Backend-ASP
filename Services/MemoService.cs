using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class MemoService(AppDbContext db, ITagService tagService, IPromptService promptService) : IMemoService
{
    public async Task<List<SummarizedMemoData>> ListAllMemosAsync(UserData user)
    {
        List<SummarizedMemoData> memos = await db.Memos.Where(m => m.UserId == user.Id)
            .Select(m => new SummarizedMemoData
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                TagCount = m.Tags.Count,
                PromptCount = m.PromptAnswers.Count
            })
            .ToListAsync();

        return memos;
    }

    public async Task<MemoData> GetMemoAsync(UserData user, int memoId)
    {
        Memo memo = await db.Memos
            .Where(m => m.UserId == user.Id && m.Id == memoId)
            .Include(m => m.Tags)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException();

        return new MemoData(memo);
    }

    public async Task<MemoData> CreateMemoAsync(UserData user, MemoCreateBody body)
    {
        bool dateExists = await db.Memos.AnyAsync(m => m.UserId == user.Id && m.Date == body.Date);
        if (dateExists)
        {
            throw new InvalidPayloadException("A memo for this date already exists.");
        }

        List<Tag> tags = [];
        if (body.Tags != null)
        {
            List<TagData> tagData = await tagService.GetTagsAsync(user, body.Tags);
            tags = [.. db.Tags.Where(t => body.Tags.Contains(t.Id))];
        }

        List<PromptAnswer> promptAnswers = [];
        if (body.Prompts != null)
        {
            promptAnswers = await CreatePromptAnswers(user, body.Prompts);
        }

        Memo memo = new()
        {
            UserId = user.Id,
            Title = body.Title,
            Date = body.Date,
            Tags = tags,
            PromptAnswers = promptAnswers
        };

        db.Memos.Add(memo);
        await db.SaveChangesAsync();

        return new MemoData(memo);
    }

    public async Task UpdateMemoAsync(UserData user, int memoId, MemoUpdateBody body)
    {
        Memo memo = await db.Memos
            .Where(m => m.UserId == user.Id && m.Id == memoId)
            .Include(m => m.Tags)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException();

        if (body.Title != null)
        {
            memo.Title = body.Title;
        }

        if (body.Tags != null)
        {
            List<TagData> tagData = await tagService.GetTagsAsync(user, body.Tags);
            List<Tag> tags = [.. db.Tags.Where(t => body.Tags.Contains(t.Id))];
            memo.Tags = tags;
        }

        if (body.Prompts != null)
        {
            List<PromptAnswer> promptAnswers = await CreatePromptAnswers(user, body.Prompts);
            memo.PromptAnswers = promptAnswers;
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteMemoAsync(UserData user, int memoId)
    {
        Memo memo = await db.Memos.FirstOrDefaultAsync(m => m.Id == memoId && m.UserId == user.Id) ?? throw new InvalidResourceException();
        db.Memos.Remove(memo);
        await db.SaveChangesAsync();
    }

    private void VerifyPromptValues(UserData user, List<PromptValueBody> promptValues, List<PromptData> prompts)
    {
        foreach (PromptValueBody pv in promptValues)
        {
            PromptData prompt = prompts.First(p => p.Id == pv.PromptId);

            if (prompt.Type == PromptType.Text && pv.Value is not string)
            {
                throw new InvalidPayloadException($"Prompt with id {prompt.Id} requires a text value.");
            }
            else if (prompt.Type == PromptType.Number && pv.Value is not double)
            {
                throw new InvalidPayloadException($"Prompt with id {prompt.Id} requires a number value.");
            }
        }
    }

    private async Task<List<PromptAnswer>> CreatePromptAnswers(UserData user, List<PromptValueBody> promptValues)
    {
        List<int> promptIds = [.. promptValues.Select(pv => pv.PromptId)];
        List<PromptData> prompts = await promptService.GetPromptsAsync(user, promptIds);

        VerifyPromptValues(user, promptValues, prompts);

        return [.. promptValues.Select(pv =>
        {
            PromptData prompt = prompts.First(p => p.Id == pv.PromptId);

            return new PromptAnswer
            {
                PromptId = prompt.Id,
                TextValue = prompt.Type == PromptType.Text ? (string)pv.Value : null,
                NumberValue = prompt.Type == PromptType.Number ? (double?)pv.Value : null
            };
        })];
    }
}