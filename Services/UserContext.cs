using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;

namespace MemoAtlas_Backend_ASP.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public User? CurrentUser => httpContextAccessor.HttpContext?.Items["User"] as User;

    public User GetRequiredUser() => CurrentUser ?? throw new UnauthenticatedException();
}