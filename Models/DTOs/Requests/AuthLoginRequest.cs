using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class AuthLoginRequest
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}