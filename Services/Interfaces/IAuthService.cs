using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IAuthService
{
    Task<UserResponse> RegisterUserAsync(AuthRegisterRequest body);
    Task<string> LoginUserAsync(AuthLoginRequest body);
    Task LogoutUserAsync(string token);
}