using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public User? CurrentUser => httpContextAccessor.HttpContext?.Items["User"] as User;

    public User GetRequiredUser() => CurrentUser ?? throw new UnauthenticatedException();
}