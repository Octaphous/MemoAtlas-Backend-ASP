namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class TagGroupStatsFilterRequest
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}