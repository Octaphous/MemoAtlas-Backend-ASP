using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IMemoService
{
    Task<List<MemoWithCountsDTO>> ListAllMemosAsync(User user);
    Task<Memo> GetMemoAsync(User user, int id);
    Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body);
    Task<Memo> UpdateMemoAsync(User user, int id, MemoUpdateRequest body);
    Task DeleteMemoAsync(User user, int id);
}
