namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptStatsFilterRequest
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}