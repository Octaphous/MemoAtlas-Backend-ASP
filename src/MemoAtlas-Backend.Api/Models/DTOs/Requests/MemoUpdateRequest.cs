using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoUpdateRequest
{
    [MinLength(1, ErrorMessage = "Title cannot be empty.")]
    public string? Title { get; set; }

    public MemoTagUpdateRequest? Tags { get; set; }

    public MemoPromptAnswerUpdateRequest? PromptAnswers { get; set; }

    public bool? Private { get; set; }

    public int? Id { get; set; }
}
