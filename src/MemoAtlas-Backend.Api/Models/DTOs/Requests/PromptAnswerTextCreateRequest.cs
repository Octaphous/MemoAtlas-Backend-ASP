namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerTextCreateRequest() : PromptAnswerCreateRequest(PromptType.Text)
{
    public required string Text { get; set; }
}