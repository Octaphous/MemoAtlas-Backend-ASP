using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IAuthService
{
    Task<UserData> RegisterUserAsync(AuthRegisterBody body);
    Task<string> LoginUserAsync(AuthLoginBody body);
    Task LogoutUserAsync(string token);
}