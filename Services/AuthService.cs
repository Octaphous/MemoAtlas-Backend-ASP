using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class AuthService(AppDbContext context) : IAuthService
{
    private readonly PasswordHasher<object?> passwordHasher = new();

    public async Task<UserDTO?> RegisterUserAsync(AuthRegisterBody body)
    {
        bool userExists = await context.Users.AnyAsync(u => u.Email == body.Email);
        if (userExists) return null;

        string passwordHash = passwordHasher.HashPassword(null, body.Password);

        User user = new()
        {
            Email = body.Email,
            PasswordHash = passwordHash
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO(user);
    }

    public async Task<string?> LoginUserAsync(AuthLoginBody body)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == body.Email);
        if (user == null) return null;

        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, body.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        Session session = new()
        {
            UserId = user.Id,
            SessionToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        context.Sessions.Add(session);
        await context.SaveChangesAsync();

        return session.SessionToken;
    }

    public async Task LogoutUserAsync(string token)
    {
        Session? session = await context.Sessions.FirstOrDefaultAsync(s => s.SessionToken == token);

        if (session != null)
        {
            context.Sessions.Remove(session);
            await context.SaveChangesAsync();
        }
    }
}