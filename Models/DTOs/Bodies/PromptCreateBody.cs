using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Bodies;

public class PromptCreateBody
{
    [Required, MinLength(1, ErrorMessage = "Question cannot be empty.")]
    public required string Question { get; set; }

    [Required, Range(0, 1, ErrorMessage = "Type must be either 0 (number) or 1 (text).")]
    public required int Type { get; set; }
}
