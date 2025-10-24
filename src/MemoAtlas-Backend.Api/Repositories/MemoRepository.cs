using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class MemoRepository(AppDbContext db) : IMemoRepository
{
    public async Task<List<Memo>> GetAllMemosAsync(User user)
    {
        return await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id)
            .Include(m => m.Tags)
                .ThenInclude(t => t.TagGroup)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<List<MemoWithCountsDTO>> GetAllMemosWithCountsAsync(User user, MemoFilterRequest filter)
    {
        return await db.Memos
            .AsNoTracking()
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id)
            .Where(m => m.Date >= filter.StartDate)
            .Where(m => m.Date <= filter.EndDate)
            .OrderByDescending(m => m.Date)
            .Select(m => new MemoWithCountsDTO
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                TagCount = m.Tags.Count,
                PromptAnswerCount = m.PromptAnswers.Count,
                Private = m.Private
            })
            .ToListAsync();
    }

    public async Task<Memo?> GetMemoAsync(User user, int memoId)
    {
        return await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id && m.Id == memoId)
            .Include(m => m.Tags)
                .ThenInclude(t => t.TagGroup)
            .Include(m => m.PromptAnswers)
                .ThenInclude(pa => pa.Prompt)
            .FirstOrDefaultAsync();
    }

    private static IQueryable<Memo> ApplyCriteriaFilters(IQueryable<Memo> query, MemoCriteriaFilterRequest filter)
    {
        if (filter.StartDate != null)
        {
            query = query.Where(m => m.Date >= filter.StartDate);
        }

        if (filter.EndDate != null)
        {
            query = query.Where(m => m.Date <= filter.EndDate);
        }

        if (filter.TagIds.Count > 0)
        {
            query = query.Where(m => m.Tags.Any(t => filter.TagIds.Contains(t.Id)));
        }

        if (filter.PromptIds.Count > 0)
        {
            query = query.Where(m => m.PromptAnswers.Any(pa => filter.PromptIds.Contains(pa.PromptId)));
        }

        if (filter.ExcludeTagIds.Count > 0)
        {
            query = query.Where(m => m.Tags.All(t => !filter.ExcludeTagIds.Contains(t.Id)));
        }

        if (filter.ExcludePromptIds.Count > 0)
        {
            query = query.Where(m => m.PromptAnswers.All(pa => !filter.ExcludePromptIds.Contains(pa.PromptId)));
        }

        return query;
    }

    public async Task<int> CountMemosByCriteriaAsync(User user, MemoCriteriaFilterRequest filter)
    {
        IQueryable<Memo> query = db.Memos
            .AsNoTracking()
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id);

        query = ApplyCriteriaFilters(query, filter);

        return await query.CountAsync();
    }

    public async Task<List<Memo>> GetMemosByCriteriaAsync(User user, MemoCriteriaFilterRequest filter, PaginationRequest pagination)
    {
        IQueryable<Memo> query = db.Memos
            .AsNoTracking()
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id);

        query = ApplyCriteriaFilters(query, filter);

        if (filter.PromptIds.Count > 0)
        {
            query = query
                .Include(m => m.PromptAnswers.Where(pa => filter.PromptIds.Contains(pa.PromptId)))
                .ThenInclude(pa => pa.Prompt);
        }

        return await query
            .OrderByDescending(m => m.Date)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
    }

    public async Task<List<Memo>> GetMemosBySearchStringAsync(User user, string query)
    {
        return await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id && (EF.Functions.Like(m.Title, $"%{query}%") || EF.Functions.Like(m.Date.ToString(), $"%{query}%")))
            .ToListAsync();
    }

    public async Task<bool> DateExistsAsync(User user, DateOnly date)
    {
        return await db.Memos.AnyAsync(m => m.UserId == user.Id && m.Date == date);
    }

    public void AddMemo(Memo memo)
    {
        db.Memos.Add(memo);
    }

    public void DeleteMemo(Memo memo)
    {
        db.Memos.Remove(memo);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}