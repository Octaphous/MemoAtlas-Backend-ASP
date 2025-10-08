using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MemoAtlas_Backend.Api.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private static readonly PasswordHasher<object> hasher = new();

    public string HashPassword(string password)
    {
        return hasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
    }

}