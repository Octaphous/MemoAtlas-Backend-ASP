namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoCriteriaFilterRequest
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public HashSet<int>? TagIds { get; set; }

    public HashSet<int>? PromptIds { get; set; }

    public HashSet<int>? ExcludeTagIds { get; set; }

    public HashSet<int>? ExcludePromptIds { get; set; }
}