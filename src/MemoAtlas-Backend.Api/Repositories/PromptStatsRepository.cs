using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class PromptStatsRepository(AppDbContext db) : IPromptStatsRepository
{
    public async Task<List<PromptStatsNumbersDTO>> GetAllNumberAnswerStatsAsync(User user, PromptStatsFilterRequest filter)
    {
        IQueryable<PromptAnswerNumber> query = db.PromptAnswerNumbers
            .AsNoTracking()
            .VisibleToUser(user)
            .Where(pan => pan.Prompt.UserId == user.Id);

        if (filter.StartDate != null)
        {
            query = query.Where(pan => pan.Memo.Date >= filter.StartDate.Value);
        }

        if (filter.EndDate != null)
        {
            query = query.Where(pan => pan.Memo.Date <= filter.EndDate.Value);
        }

        if (!user.PrivateMode)
        {
            query = query.Where(pan => !pan.Prompt.Private && !pan.Memo.Private);
        }

        return await query
            .GroupBy(pan => pan.Prompt.Id)
            .Select(g => new PromptStatsNumbersDTO
            {
                PromptId = g.Key,
                Type = g.First().Prompt.Type,
                Question = g.First().Prompt.Question,
                Private = g.First().Prompt.Private,
                Count = g.Count(),
                Average = g.Average(pan => pan.Number),
                Maximum = g.Max(pan => pan.Number),
                Minimum = g.Min(pan => pan.Number),
                Total = g.Sum(pan => pan.Number)
            }).ToListAsync();
    }
}