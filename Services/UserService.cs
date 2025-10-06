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

public class UserService(AppDbContext db) : IUserService
{
    private readonly PasswordHasher<User?> passwordHasher = new();

    public async Task<User> CreateUserAsync(UserCreateRequest body)
    {
        bool userExists = await db.Users.AnyAsync(u => u.Email == body.Email);
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

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserFromCredentialsAsync(UserLoginRequest body)
    {
        User user = await db.Users.FirstOrDefaultAsync(u => u.Email == body.Email)
            ?? throw new UnauthenticatedException("Invalid email or password.");

        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, body.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthenticatedException("Invalid email or password.");
        }

        return user;
    }
}