using MemoAtlas_Backend_ASP.Models.DTOs;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IUserContext
{
    UserData? CurrentUser { get; }
    UserData GetRequiredUser();
}
