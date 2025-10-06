using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptAnswerDTO
{
    public required int PromptId { get; set; }
    public required PromptType Type { get; set; }
    public required string Question { get; set; }
    public required object? Value { get; set; }
}