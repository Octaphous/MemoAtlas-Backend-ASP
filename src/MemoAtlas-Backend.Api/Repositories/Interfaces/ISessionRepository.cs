using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetSessionByTokenAsync(string token);

    void AddSession(Session session);
    void DeleteSession(Session session);
    Task SaveChangesAsync();
}