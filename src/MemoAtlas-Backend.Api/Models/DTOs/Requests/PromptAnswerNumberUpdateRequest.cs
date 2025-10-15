namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerNumberUpdateRequest() : PromptAnswerUpdateRequest(PromptType.Number)
{
    public double? Answer { get; set; }
}