using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class UserResponse(User user)
{
    public int Id { get; set; } = user.Id;
    public string Email { get; set; } = user.Email;
}
