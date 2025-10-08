using Microsoft.AspNetCore.Identity;

namespace MemoAtlas_Backend.Api.Utilities;

public static class PasswordUtility
{
    private static readonly PasswordHasher<object> hasher = new();

    public static string HashPassword(string password)
    {
        return hasher.HashPassword(null!, password);
    }

    public static bool VerifyPassword(string hashedPassword, string password)
    {
        PasswordVerificationResult result = hasher.VerifyHashedPassword(null!, hashedPassword, password);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}