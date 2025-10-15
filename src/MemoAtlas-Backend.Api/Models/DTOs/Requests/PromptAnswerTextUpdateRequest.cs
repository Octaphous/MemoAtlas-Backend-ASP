namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerTextUpdateRequest() : PromptAnswerUpdateRequest(PromptType.Text)
{
    public string? Text { get; set; }
}