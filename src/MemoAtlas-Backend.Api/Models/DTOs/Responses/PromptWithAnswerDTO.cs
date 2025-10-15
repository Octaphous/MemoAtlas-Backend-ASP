namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptWithAnswerDTO
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required PromptType Type { get; set; }
    public required bool Private { get; set; }
    public required IEnumerable<PromptAnswerDTO> Answers { get; set; }
}