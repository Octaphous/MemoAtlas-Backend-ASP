using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IUserContext
{
    User? CurrentUser { get; }
    User GetRequiredUser();
}
