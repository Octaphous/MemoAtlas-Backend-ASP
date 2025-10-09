using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<bool> UserExistsAsync(string email)
    {
        return await db.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public void AddUser(User user)
    {
        db.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}