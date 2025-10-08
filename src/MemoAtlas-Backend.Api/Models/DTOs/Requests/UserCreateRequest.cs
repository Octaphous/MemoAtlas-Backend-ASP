using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class UserCreateRequest
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, MinLength(6)]
    public required string Password { get; set; }
}