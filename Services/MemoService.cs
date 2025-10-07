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
            promptAnswers = await promptService.CreatePromptAnswersAsync(user, body.PromptAnswers);
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

        if (body.Tags?.Add != null && body.Tags?.Remove != null)
        {
            var overlap = body.Tags.Add.Intersect(body.Tags.Remove).Any();
            if (overlap)
            {
                throw new InvalidPayloadException("Cannot add and remove the same tag(s) in a single request.");
            }
        }

        if (body.Tags?.Add != null)
        {
            List<Tag> tags = await tagService.GetTagsAsync(user, body.Tags.Add);
            HashSet<int> existingIds = memo.Tags.Select(t => t.Id).ToHashSet();
            memo.Tags.AddRange(tags.Where(t => !existingIds.Contains(t.Id)));
        }

        if (body.Tags?.Remove != null)
        {
            memo.Tags.RemoveAll(t => body.Tags.Remove.Contains(t.Id));
        }

        if (body.PromptAnswers?.Add != null && body.PromptAnswers?.Remove != null)
        {
            var overlap = body.PromptAnswers.Add.Select(pa => pa.PromptId).Intersect(body.PromptAnswers.Remove).Any();
            if (overlap)
            {
                throw new InvalidPayloadException("Cannot add and remove the same prompt answer(s) in a single request.");
            }
        }

        if (body.PromptAnswers?.Add != null)
        {
            Dictionary<int, PromptAnswer> existingAnswers = memo.PromptAnswers.ToDictionary(pa => pa.PromptId);
            List<PromptAnswer> newAnswers = await promptService.CreatePromptAnswersAsync(user, body.PromptAnswers.Add);

            foreach (PromptAnswer newAnswer in newAnswers)
            {
                if (existingAnswers.TryGetValue(newAnswer.PromptId, out var existing))
                {
                    existing.TextValue = newAnswer.TextValue;
                    existing.NumberValue = newAnswer.NumberValue;
                }
                else
                {
                    memo.PromptAnswers.Add(newAnswer);
                }
            }
        }

        if (body.PromptAnswers?.Remove != null)
        {
            memo.PromptAnswers.RemoveAll(pa => body.PromptAnswers.Remove.Contains(pa.PromptId));
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
}