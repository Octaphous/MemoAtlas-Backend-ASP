using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.Configurations;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoAtlas_Backend.Api.Services;

public class SessionService(ISessionRepository sessionRepository, IOptions<AuthOptions> authOptions) : ISessionService
{
    public async Task<Session> GetSessionByTokenAsync(string token)
    {
        Session session = await sessionRepository.GetSessionByTokenAsync(token)
            ?? throw new InvalidResourceException("Session not found.");

        return session;
    }

    public async Task<Session> CreateSessionAsync(User user)
    {
        Session session = new()
        {
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(authOptions.Value.SessionDurationHours)
        };

        sessionRepository.AddSession(session);
        await sessionRepository.SaveChangesAsync();

        return session;
    }

    public async Task DeleteSessionAsync(string token)
    {
        Session session = await GetSessionByTokenAsync(token);
        sessionRepository.DeleteSession(session);
        await sessionRepository.SaveChangesAsync();
    }
}