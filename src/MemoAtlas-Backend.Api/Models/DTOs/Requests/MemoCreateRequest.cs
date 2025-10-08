using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoCreateRequest
{
    [Required, MinLength(1, ErrorMessage = "Title cannot be empty.")]
    public required string Title { get; set; }

    [Required]
    public required DateOnly Date { get; set; }

    [MinLength(1, ErrorMessage = "At least one tag is required if tags are provided.")]
    public IEnumerable<int>? Tags { get; set; }

    [MinLength(1, ErrorMessage = "At least one prompt is required if prompts are provided.")]
    public IEnumerable<PromptAnswerCreateRequest>? PromptAnswers { get; set; }

    [Required]
    public required bool Private { get; set; }
}
