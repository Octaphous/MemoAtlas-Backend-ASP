using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class TagGroupCreateRequest
{
    [Required, MinLength(1, ErrorMessage = "Name cannot be empty.")]
    public required string Name { get; set; }

    [Required]
    public required string Color { get; set; }

    [Required]
    public required bool Private { get; set; }
}
