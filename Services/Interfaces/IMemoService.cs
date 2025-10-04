using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces
{
    public interface IMemoService
    {
        Task<List<MemoData>> GetAllMemosAsync();
        Task<MemoData> CreateMemoAsync(MemoCreateBody body);
        Task DeleteMemoAsync(int id);
    }
}