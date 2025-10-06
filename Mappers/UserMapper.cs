using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

public static class UserMapper
{
    public static UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        Email = user.Email
    };
}