namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class MemoWithPromptAnswerDTO
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required object? Value { get; set; }
}