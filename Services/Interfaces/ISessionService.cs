using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface ISessionService
{
    Task<Session> GetSessionByTokenAsync(string token);
    Task<Session> CreateSessionAsync(User user);
    Task DeleteSessionAsync(string token);
}