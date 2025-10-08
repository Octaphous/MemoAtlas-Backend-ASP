using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

public static class UserMapper
{
    public static UserDTO ToDTO(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        PrivateMode = user.PrivateMode
    };
}