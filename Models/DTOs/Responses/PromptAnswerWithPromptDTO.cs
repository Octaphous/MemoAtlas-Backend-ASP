namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptAnswerWithPromptDTO
{
    public required int Id { get; set; }
    public required PromptDTO Prompt { get; set; }
    public required object? Value { get; set; }
    public required bool Private { get; set; }
}