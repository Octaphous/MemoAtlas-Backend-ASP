using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);

    void AddUser(User user);
    Task SaveChangesAsync();
}