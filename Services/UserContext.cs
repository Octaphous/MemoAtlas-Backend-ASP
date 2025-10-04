using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;

namespace MemoAtlas_Backend_ASP.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserData? CurrentUser => httpContextAccessor.HttpContext?.Items["User"] as UserData;

    public UserData GetRequiredUser() => CurrentUser ?? throw new UnauthorizedAccessException("User not authenticated");
}