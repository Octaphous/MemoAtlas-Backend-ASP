using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class UserEnablePrivateRequest
{
    [Required]
    public required string Password { get; set; }
}