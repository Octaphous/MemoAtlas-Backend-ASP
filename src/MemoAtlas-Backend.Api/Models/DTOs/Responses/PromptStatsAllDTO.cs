namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptStatsAllDTO
{
    public required List<PromptStatsNumbersDTO> NumberPrompts { get; set; }
}