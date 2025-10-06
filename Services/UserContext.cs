using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Services.Interfaces;

namespace MemoAtlas_Backend_ASP.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserResponse? CurrentUser => httpContextAccessor.HttpContext?.Items["User"] as UserResponse;

    public UserResponse GetRequiredUser() => CurrentUser ?? throw new UnauthenticatedException();
}