using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class AuthRegisterRequest
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, MinLength(6)]
    public required string Password { get; set; }
}