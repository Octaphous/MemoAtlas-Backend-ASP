namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptAnswerTextDTO : PromptAnswerDTO
{
    public required string Text { get; set; }
}