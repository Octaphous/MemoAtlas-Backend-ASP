using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;

namespace MemoAtlas_Backend.Api.Services;

public class MemoService(IMemoRepository memoRepository, ITagService tagService, IPromptAnswerService promptAnswerService) : IMemoService
{
    public async Task<IEnumerable<MemoWithCountsDTO>> ListAllMemosAsync(User user, MemoFilterRequest filter)
    {
        Validators.ValidateOptionalDateSpan(filter.StartDate, filter.EndDate);

        return await memoRepository.GetAllMemosWithCountsAsync(user, filter);
    }

    public async Task<Memo> GetMemoAsync(User user, int memoId)
    {
        Memo memo = await memoRepository.GetMemoAsync(user, memoId) ?? throw new InvalidResourceException();

        if (!user.PrivateMode)
        {
            memo.Tags = memo.Tags.Where(tag => !tag.Private && !tag.TagGroup.Private).ToList();
            memo.PromptAnswers = memo.PromptAnswers.Where(pa => !pa.Private && !pa.Prompt.Private).ToList();
        }

        return memo;
    }

    public async Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body)
    {
        bool dateExists = await memoRepository.DateExistsAsync(user, body.Date);
        if (dateExists)
        {
            throw new InvalidPayloadException("A memo for this date already exists.");
        }

        List<Tag> tags = [];
        if (body.Tags != null)
        {
            tags = (await tagService.GetTagsAsync(user, body.Tags.ToHashSet())).ToList();
        }

        List<PromptAnswer> promptAnswers = [];
        if (body.PromptAnswers != null)
        {
            await promptAnswerService.ValidatePromptAnswerRequests(user, body.PromptAnswers);
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

        memoRepository.AddMemo(memo);
        await memoRepository.SaveChangesAsync();

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
            List<Tag> tags = (await tagService.GetTagsAsync(user, body.Tags.Add.ToHashSet())).ToList();
            HashSet<int> existingIds = memo.Tags.Select(t => t.Id).ToHashSet();
            memo.Tags.AddRange(tags.Where(t => !existingIds.Contains(t.Id)));
        }

        if (body.Tags?.Remove != null)
        {
            memo.Tags.RemoveAll(t => body.Tags.Remove.Contains(t.Id));
        }

        if (body.PromptAnswers?.Add != null)
        {
            await promptAnswerService.ValidatePromptAnswerRequests(user, body.PromptAnswers.Add);
            IEnumerable<PromptAnswer> promptAnswers = await promptAnswerService.BuildPromptAnswersAsync(user, body.PromptAnswers.Add);
            memo.PromptAnswers.AddRange(promptAnswers);
        }

        if (body.PromptAnswers?.Update != null)
        {
            promptAnswerService.SetUpdatedPromptAnswers(memo.PromptAnswers, body.PromptAnswers.Update);
        }

        if (body.PromptAnswers?.Remove != null)
        {
            memo.PromptAnswers.RemoveAll(pa => body.PromptAnswers.Remove.Contains(pa.Id));
        }

        await memoRepository.SaveChangesAsync();
        return memo;
    }

    public async Task DeleteMemoAsync(User user, int memoId)
    {
        Memo memo = await GetMemoAsync(user, memoId);
        memoRepository.DeleteMemo(memo);
        await memoRepository.SaveChangesAsync();
    }
}