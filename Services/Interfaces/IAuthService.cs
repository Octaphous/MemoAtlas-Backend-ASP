using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterUserAsync(AuthRegisterRequest body);
    Task<Session> LoginUserAsync(AuthLoginRequest body);
    Task LogoutUserAsync(string token);
}