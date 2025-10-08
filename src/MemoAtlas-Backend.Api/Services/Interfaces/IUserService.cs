using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UserCreateRequest body);
    Task<User> GetUserFromCredentialsAsync(UserLoginRequest body);
    Task EnablePrivateModeAsync(User user, UserEnablePrivateRequest body);
    Task DisablePrivateModeAsync(User user);
}