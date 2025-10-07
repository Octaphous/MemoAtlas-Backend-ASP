namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptDTO
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required PromptType Type { get; set; }
}
