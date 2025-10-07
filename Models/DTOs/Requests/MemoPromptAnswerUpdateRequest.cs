using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class MemoPromptAnswerUpdateRequest
{
    [MinLength(1, ErrorMessage = "No prompt answers to add were provided.")]
    public IEnumerable<PromptAnswerRequest>? Add { get; set; }

    [MinLength(1, ErrorMessage = "No prompt answers to remove were provided.")]
    public IEnumerable<int>? Remove { get; set; }
}
