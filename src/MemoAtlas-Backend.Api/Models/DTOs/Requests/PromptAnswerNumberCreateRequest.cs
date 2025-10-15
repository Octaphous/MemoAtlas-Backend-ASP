namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerNumberCreateRequest() : PromptAnswerCreateRequest(PromptType.Number)
{
    public required double Number { get; set; }
}