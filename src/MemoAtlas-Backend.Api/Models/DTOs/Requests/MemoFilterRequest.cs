namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoFilterRequest
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public HashSet<int>? TagIds { get; set; }
}