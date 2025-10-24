namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class MemoWithAnswersDTO
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required bool Private { get; set; }
    public required IEnumerable<PromptWithAnswerDTO> Prompts { get; set; }
}