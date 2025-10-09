using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class SessionRepository(AppDbContext db) : ISessionRepository
{
    public async Task<Session?> GetSessionByTokenAsync(string token)
    {
        return await db.Sessions.Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);
    }

    public void AddSession(Session session)
    {

        db.Sessions.Add(session);
    }

    public void DeleteSession(Session session)
    {
        db.Sessions.Remove(session);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}