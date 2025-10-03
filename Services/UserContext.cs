using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;

namespace MemoAtlas_Backend_ASP.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserDTO? CurrentUser => httpContextAccessor.HttpContext?.Items["User"] as UserDTO;
}