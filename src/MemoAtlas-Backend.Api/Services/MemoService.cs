using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Services;

public class MemoService(AppDbContext db, ITagService tagService, IPromptAnswerService promptAnswerService) : IMemoService
{
    public async Task<List<MemoWithCountsDTO>> ListAllMemosAsync(User user)
    {
        List<MemoWithCountsDTO> memos = await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id)
            .Select(m => new MemoWithCountsDTO
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                TagCount = m.Tags.Count,
                PromptAnswerCount = m.PromptAnswers.Count,
                Private = m.Private
            }).ToListAsync();

        return memos;
    }

    public async Task<Memo> GetMemoAsync(User user, int memoId)
    {
        Memo memo = await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id && m.Id == memoId)
            .Include(m => m.Tags)
                .ThenInclude(t => t.TagGroup)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .FirstOrDefaultAsync() ?? throw new InvalidResourceException();

        // If user is not in private mode, remove any tags that are private or belong to a private tag group
        memo.Tags = memo.Tags.Where(tag => (!tag.Private && !tag.TagGroup.Private) || user.PrivateMode).ToList();

        // If user is not in private mode, remove any prompt answers that are private or belong to a private prompt
        memo.PromptAnswers = memo.PromptAnswers.Where(pa => (!pa.Private && !pa.Prompt.Private) || user.PrivateMode).ToList();

        return memo;
    }

    public async Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body)
    {
        bool dateExists = await db.Memos
            .AnyAsync(m => m.UserId == user.Id && m.Date == body.Date);

        if (dateExists)
        {
            throw new InvalidPayloadException("A memo for this date already exists.");
        }

        List<Tag> tags = [];
        if (body.Tags != null)
        {
            tags = await tagService.GetTagsAsync(user, body.Tags.ToHashSet());
        }

        List<PromptAnswer> promptAnswers = [];
        if (body.PromptAnswers != null)
        {
            promptAnswers = (await promptAnswerService.BuildPromptAnswersAsync(user, body.PromptAnswers)).ToList();
        }

        Memo memo = new()
        {
            UserId = user.Id,
            Title = body.Title,
            Date = body.Date,
            Tags = tags,
            PromptAnswers = promptAnswers,
            Private = body.Private
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

        if (body.Private != null)
        {
            memo.Private = body.Private.Value;
        }

        if (body.Tags?.Add != null && body.Tags?.Remove != null)
        {
            var overlap = body.Tags.Add.Intersect(body.Tags.Remove).Any();
            if (overlap)
            {
                throw new InvalidPayloadException("Cannot add and remove the same tag(s) in a single request.");
            }
        }

        if (body.PromptAnswers?.Update != null && body.PromptAnswers?.Remove != null)
        {
            var overlap = body.PromptAnswers.Update.Select(pa => pa.Id).Intersect(body.PromptAnswers.Remove).Any();
            if (overlap)
            {
                throw new InvalidPayloadException("Cannot update and remove the same prompt answer(s) in a single request.");
            }
        }

        if (body.Tags?.Add != null)
        {
            List<Tag> tags = await tagService.GetTagsAsync(user, body.Tags.Add.ToHashSet());
            HashSet<int> existingIds = memo.Tags.Select(t => t.Id).ToHashSet();
            memo.Tags.AddRange(tags.Where(t => !existingIds.Contains(t.Id)));
        }

        if (body.Tags?.Remove != null)
        {
            memo.Tags.RemoveAll(t => body.Tags.Remove.Contains(t.Id));
        }

        if (body.PromptAnswers?.Add != null)
        {
            IEnumerable<PromptAnswer> promptAnswers = await promptAnswerService.BuildPromptAnswersAsync(user, body.PromptAnswers.Add);
            memo.PromptAnswers.AddRange(promptAnswers);
        }

        if (body.PromptAnswers?.Update != null)
        {
            promptAnswerService.SetUpdatedPromptAnswers(memo, body.PromptAnswers.Update);
        }

        if (body.PromptAnswers?.Remove != null)
        {
            memo.PromptAnswers.RemoveAll(pa => body.PromptAnswers.Remove.Contains(pa.Id));
        }

        await db.SaveChangesAsync();
        return memo;
    }

    public async Task DeleteMemoAsync(User user, int memoId)
    {
        Memo memo = await db.Memos
            .VisibleToUser(user)
            .FirstOrDefaultAsync(m => m.Id == memoId && m.UserId == user.Id) ?? throw new InvalidResourceException();

        db.Memos.Remove(memo);
        await db.SaveChangesAsync();
    }
}