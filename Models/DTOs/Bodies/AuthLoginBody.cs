using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Bodies;

public class AuthLoginBody
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}