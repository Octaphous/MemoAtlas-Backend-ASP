namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptStatsNumbersDTO
{
    public required int PromptId { get; set; }
    public required string Question { get; set; }
    public required PromptType Type { get; set; }
    public required bool Private { get; set; }
    public required int Count { get; set; }
    public required double Average { get; set; }
    public required double Minimum { get; set; }
    public required double Maximum { get; set; }
    public required double Total { get; set; }
}