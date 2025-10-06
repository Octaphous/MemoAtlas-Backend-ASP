using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IUserContext
{
    UserResponse? CurrentUser { get; }
    UserResponse GetRequiredUser();
}
