using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UserCreateRequest body);
    Task<User> GetUserFromCredentialsAsync(UserLoginRequest body);
    Task EnablePrivateModeAsync(User user, UserEnablePrivateRequest body);
    Task DisablePrivateModeAsync(User user);
}