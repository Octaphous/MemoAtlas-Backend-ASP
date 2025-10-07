using System.Net;
using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using MemoAtlas_Backend_ASP.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<User> CreateUserAsync(UserCreateRequest body)
    {
        bool userExists = await db.Users.AnyAsync(u => u.Email == body.Email);
        if (userExists)
        {
            throw new ResourceConflictException("A user with this email already exists.");
        }

        User user = new()
        {
            Email = body.Email,
            PasswordHash = PasswordUtility.HashPassword(body.Password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserFromCredentialsAsync(UserLoginRequest body)
    {
        User user = await db.Users.FirstOrDefaultAsync(u => u.Email == body.Email)
            ?? throw new UnauthenticatedException("Invalid email or password.");

        bool passwordValid = PasswordUtility.VerifyPassword(user.PasswordHash, body.Password);
        if (!passwordValid)
        {
            throw new UnauthenticatedException("Invalid email or password.");
        }

        return user;
    }

    public async Task EnablePrivateModeAsync(User user, UserEnablePrivateRequest body)
    {
        bool passwordValid = PasswordUtility.VerifyPassword(user.PasswordHash, body.Password);
        if (!passwordValid)
        {
            throw new UnauthenticatedException("Invalid password.");
        }

        user.PrivateMode = true;
        await db.SaveChangesAsync();
    }

    public async Task DisablePrivateModeAsync(User user)
    {
        user.PrivateMode = false;
        await db.SaveChangesAsync();
    }
}