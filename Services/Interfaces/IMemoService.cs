using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IMemoService
{
    Task<List<MemoSummarizedResponse>> ListAllMemosAsync(UserResponse user);
    Task<MemoResponse> GetMemoAsync(UserResponse user, int id);
    Task<MemoResponse> CreateMemoAsync(UserResponse user, MemoCreateRequest body);
    Task UpdateMemoAsync(UserResponse user, int id, MemoUpdateRequest body);
    Task DeleteMemoAsync(UserResponse user, int id);
}
