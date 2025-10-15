namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerTextUpdateRequest() : PromptAnswerUpdateRequest(PromptType.Text)
{
    public string? Answer { get; set; }
}