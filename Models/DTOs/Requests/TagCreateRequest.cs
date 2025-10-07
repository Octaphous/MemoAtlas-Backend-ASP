using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class TagCreateRequest
{
    [Required, MinLength(1, ErrorMessage = "Name cannot be empty.")]
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required]
    public required int GroupId { get; set; }
}
