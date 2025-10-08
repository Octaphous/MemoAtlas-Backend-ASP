using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface ISessionService
{
    Task<Session> GetSessionByTokenAsync(string token);
    Task<Session> CreateSessionAsync(User user);
    Task DeleteSessionAsync(string token);
}