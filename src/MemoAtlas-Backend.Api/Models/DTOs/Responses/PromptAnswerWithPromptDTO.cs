namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptAnswerWithPromptDTO
{
    public required PromptAnswerDTO Data { get; set; }
    public required PromptDTO Prompt { get; set; }
}