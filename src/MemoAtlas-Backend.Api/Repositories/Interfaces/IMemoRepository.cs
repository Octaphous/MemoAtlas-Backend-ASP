using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface IMemoRepository
{
    Task<List<MemoWithCountsDTO>> GetAllMemosWithCountsAsync(User user, MemoFilterRequest filter);
    Task<Memo?> GetMemoAsync(User user, int memoId);
    Task<bool> DateExistsAsync(User user, DateOnly date);

    void AddMemo(Memo memo);
    void DeleteMemo(Memo memo);
    Task SaveChangesAsync();
}