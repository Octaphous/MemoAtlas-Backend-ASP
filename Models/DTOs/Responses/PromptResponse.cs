using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptResponse
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required PromptType Type { get; set; }
}
