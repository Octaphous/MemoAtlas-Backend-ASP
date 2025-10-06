namespace MemoAtlas_Backend_ASP.Models.Entities;

public class PromptAnswer
{
    public int Id { get; set; }

    public int MemoId { get; set; }

    public int PromptId { get; set; }

    public string? TextValue { get; set; }

    public double? NumberValue { get; set; }

    // Navigation properties
    public Memo Memo { get; set; } = null!;
    public Prompt Prompt { get; set; } = null!;
}

