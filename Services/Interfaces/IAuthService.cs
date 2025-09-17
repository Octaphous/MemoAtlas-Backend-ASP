using MemoAtlas_Backend_ASP.Models.DTOs;

namespace MemoAtlas_Backend_ASP.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterUserAsync(AuthRegisterBody body);
        Task<string?> LoginUserAsync(AuthLoginBody body);
        Task LogoutUserAsync(string token);
    }
}