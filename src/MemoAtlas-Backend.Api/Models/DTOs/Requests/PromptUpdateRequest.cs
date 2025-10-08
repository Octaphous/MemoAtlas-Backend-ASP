using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptUpdateRequest
{
    [MinLength(1, ErrorMessage = "Question cannot be empty.")]
    public string? Question { get; set; }

    public bool? Private { get; set; }
}
