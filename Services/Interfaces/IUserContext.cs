using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Services.Interfaces;

public interface IUserContext
{
    User? CurrentUser { get; }
    User GetRequiredUser();
}
