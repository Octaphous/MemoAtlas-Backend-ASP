namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerNumberCreateRequest() : PromptAnswerCreateRequest(PromptType.Number)
{
    public required double Answer { get; set; }
}