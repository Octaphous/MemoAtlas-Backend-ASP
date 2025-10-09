using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Services;

public class UserService(IUserRepository userRepository, IPasswordHasherService passwordHasherService) : IUserService
{
    public async Task<User> CreateUserAsync(UserCreateRequest body)
    {
        bool userExists = await userRepository.UserExistsAsync(body.Email);
        if (userExists)
        {
            throw new ResourceConflictException("A user with this email already exists.");
        }

        User user = new()
        {
            Email = body.Email,
            PasswordHash = passwordHasherService.HashPassword(body.Password)
        };

        userRepository.AddUser(user);
        await userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserFromCredentialsAsync(UserLoginRequest body)
    {
        User user = await userRepository.GetUserByEmailAsync(body.Email)
            ?? throw new UnauthenticatedException("Invalid email or password.");

        bool passwordValid = passwordHasherService.VerifyPassword(user.PasswordHash, body.Password);
        if (!passwordValid)
        {
            throw new UnauthenticatedException("Invalid email or password.");
        }

        return user;
    }

    public async Task EnablePrivateModeAsync(User user, UserEnablePrivateRequest body)
    {
        bool passwordValid = passwordHasherService.VerifyPassword(user.PasswordHash, body.Password);
        if (!passwordValid)
        {
            throw new UnauthenticatedException("Invalid password.");
        }

        user.PrivateMode = true;
        await userRepository.SaveChangesAsync();
    }

    public async Task DisablePrivateModeAsync(User user)
    {
        user.PrivateMode = false;
        await userRepository.SaveChangesAsync();
    }
}