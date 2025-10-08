using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoPromptAnswerUpdateRequest
{
    [MinLength(1, ErrorMessage = "No prompt answers to add were provided.")]
    public IEnumerable<PromptAnswerCreateRequest>? Add { get; set; }

    [MinLength(1, ErrorMessage = "No prompt answers to update were provided.")]
    public IEnumerable<PromptAnswerUpdateRequest>? Update { get; set; }

    [MinLength(1, ErrorMessage = "No prompt answers to remove were provided.")]
    public IEnumerable<int>? Remove { get; set; }
}
