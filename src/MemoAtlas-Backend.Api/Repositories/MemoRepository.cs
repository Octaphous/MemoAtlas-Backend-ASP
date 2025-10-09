using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class MemoRepository(AppDbContext db) : IMemoRepository
{
    public async Task<List<MemoWithCountsDTO>> GetAllMemosAsync(User user)
    {
        return await db.Memos
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