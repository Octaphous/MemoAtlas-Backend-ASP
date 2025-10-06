using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class UserDTO
{
    public required int Id { get; set; }
    public required string Email { get; set; }
}
