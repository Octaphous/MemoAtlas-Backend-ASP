namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptWithMemosDTO
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required PromptType Type { get; set; }
    public required IEnumerable<MemoWithPromptAnswerDTO> Memos { get; set; }
    public required bool Private { get; set; }
}
