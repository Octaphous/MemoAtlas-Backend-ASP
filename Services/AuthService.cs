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

public class AuthService(AppDbContext context) : IAuthService
{
    private readonly PasswordHasher<object?> passwordHasher = new();

    public async Task<UserResponse> RegisterUserAsync(AuthRegisterRequest body)
    {
        bool userExists = await context.Users.AnyAsync(u => u.Email == body.Email);
        if (userExists)
        {
            throw new ResourceConflictException("A user with this email already exists.");
        }

        string passwordHash = passwordHasher.HashPassword(null, body.Password);

        User user = new()
        {
            Email = body.Email,
            PasswordHash = passwordHash
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponse(user);
    }

    public async Task<string> LoginUserAsync(AuthLoginRequest body)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == body.Email);
        if (user == null)
        {
            throw new UnauthenticatedException("Invalid email or password.");
        }

        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, body.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthenticatedException("Invalid email or password.");
        }

        Session session = new()
        {
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        context.Sessions.Add(session);
        await context.SaveChangesAsync();

        return session.Token;
    }

    public async Task LogoutUserAsync(string token)
    {
        Session session = await context.Sessions.FirstOrDefaultAsync(s => s.Token == token) ?? throw new UnauthenticatedException("Invalid session token.");

        context.Sessions.Remove(session);
        await context.SaveChangesAsync();
    }
}