using System.Net;
using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class SessionService(AppDbContext db) : ISessionService
{
    public async Task<Session> GetSessionByTokenAsync(string token)
    {
        Session session = await db.Sessions.Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow)
            ?? throw new InvalidResourceException("Session not found.");

        return session;
    }

    public async Task<Session> CreateSessionAsync(User user)
    {
        Session session = new()
        {
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        db.Sessions.Add(session);
        await db.SaveChangesAsync();

        return session;
    }

    public async Task DeleteSessionAsync(string token)
    {
        Session session = await db.Sessions.FirstOrDefaultAsync(s => s.Token == token)
            ?? throw new InvalidResourceException("Session not found.");

        db.Sessions.Remove(session);
        await db.SaveChangesAsync();
    }
}