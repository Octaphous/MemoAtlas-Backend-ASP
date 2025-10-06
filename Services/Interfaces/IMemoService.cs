using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IMemoService
{
    Task<List<MemoSummarizedResponse>> ListAllMemosAsync(User user);
    Task<Memo> GetMemoAsync(User user, int id);
    Task<Memo> CreateMemoAsync(User user, MemoCreateRequest body);
    Task<Memo> UpdateMemoAsync(User user, int id, MemoUpdateRequest body);
    Task DeleteMemoAsync(User user, int id);
}
