using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class MemoService(AppDbContext db, ITagService tagService, IPromptService promptService) : IMemoService
{
    public async Task<List<MemoWithCountsDTO>> ListAllMemosAsync(User user)
    {
        List<MemoWithCountsDTO> memos = await db.Memos.Where(m => m.UserId == user.Id)
            .Select(m => new MemoWithCountsDTO
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                TagCount = m.Tags.Count,
                PromptAnswerCount = m.PromptAnswers.Count
            })
            .ToListAsync();

        return memos;
    }

    public async Task<Memo> GetMemoAsync(User user, int memoId)
    {
        Memo memo = await db.Memos
            .Where(m => m.UserId == user.Id && m.Id == memoId)
            .Include(m => m.Tags)
                .ThenInclude(t => t.TagGroup)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException();

        return memo;
    }

    public async Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body)
    {
        bool dateExists = await db.Memos.AnyAsync(m => m.UserId == user.Id && m.Date == body.Date);
        if (dateExists)
        {
            throw new InvalidPayloadException("A memo for this date already exists.");
        }

        List<Tag> tags = [];
        if (body.Tags != null)
        {
            tags = await tagService.GetTagsAsync(user, body.Tags);
        }

        List<PromptAnswer> promptAnswers = [];
        if (body.PromptAnswers != null)
        {
            promptAnswers = await CreatePromptAnswersAsync(user, body.PromptAnswers);
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

        return memo;
    }

    public async Task<Memo> UpdateMemoAsync(User user, int memoId, MemoUpdateRequest body)
    {
        Memo memo = await GetMemoAsync(user, memoId);

        if (body.Title != null)
        {
            memo.Title = body.Title;
        }

        if (body.Tags != null)
        {
            List<Tag> tags = await tagService.GetTagsAsync(user, body.Tags);
            memo.Tags = tags;
        }

        if (body.PromptAnswers != null)
        {
            List<PromptAnswer> promptAnswers = await CreatePromptAnswersAsync(user, body.PromptAnswers);
            memo.PromptAnswers = promptAnswers;
        }

        await db.SaveChangesAsync();
        return memo;
    }

    public async Task DeleteMemoAsync(User user, int memoId)
    {
        Memo memo = await db.Memos.FirstOrDefaultAsync(m => m.Id == memoId && m.UserId == user.Id) ?? throw new InvalidResourceException();
        db.Memos.Remove(memo);
        await db.SaveChangesAsync();
    }

    private void VerifyPromptAnswers(List<PromptAnswerRequest> promptAnswers, List<Prompt> prompts)
    {
        foreach (PromptAnswerRequest pv in promptAnswers)
        {
            Prompt prompt = prompts.First(p => p.Id == pv.PromptId);

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

    private async Task<List<PromptAnswer>> CreatePromptAnswersAsync(User user, List<PromptAnswerRequest> promptAnswers)
    {
        List<int> promptIds = [.. promptAnswers.Select(pv => pv.PromptId)];
        List<Prompt> prompts = await promptService.GetPromptsAsync(user, promptIds);

        VerifyPromptAnswers(promptAnswers, prompts);

        return [.. promptAnswers.Select(pv =>
        {
            Prompt prompt = prompts.First(p => p.Id == pv.PromptId);

            return new PromptAnswer
            {
                PromptId = prompt.Id,
                TextValue = prompt.Type == PromptType.Text ? (string)pv.Value : null,
                NumberValue = prompt.Type == PromptType.Number ? (double?)pv.Value : null
            };
        })];
    }
}