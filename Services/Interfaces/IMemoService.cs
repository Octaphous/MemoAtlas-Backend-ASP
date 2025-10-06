using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IMemoService
{
    Task<List<SummarizedMemoData>> GetAllMemosAsync(UserData user);
    Task<MemoData> GetMemoAsync(UserData user, int id);
    Task<MemoData> CreateMemoAsync(UserData user, MemoCreateBody body);
    Task UpdateMemoAsync(UserData user, int id, MemoUpdateBody body);
    Task DeleteMemoAsync(UserData user, int id);
}
