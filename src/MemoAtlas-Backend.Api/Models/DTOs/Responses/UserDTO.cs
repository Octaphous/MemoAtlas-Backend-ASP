namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class UserDTO
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required bool PrivateMode { get; set; }
}
