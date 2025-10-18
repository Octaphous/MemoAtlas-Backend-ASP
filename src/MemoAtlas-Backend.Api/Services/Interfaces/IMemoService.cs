using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IMemoService
{
    Task<IEnumerable<MemoWithCountsDTO>> ListAllMemosAsync(User user, MemoFilterRequest filter);
    Task<Memo> GetMemoAsync(User user, int id);
    Task<List<Memo>> SearchMemosAsync(User user, string query);
    Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body);
    Task<Memo> UpdateMemoAsync(User user, int id, MemoUpdateRequest body);
    Task DeleteMemoAsync(User user, int id);
}
