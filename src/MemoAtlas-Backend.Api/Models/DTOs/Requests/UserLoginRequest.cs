using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class UserLoginRequest
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}