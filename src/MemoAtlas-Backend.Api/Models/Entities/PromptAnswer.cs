using MemoAtlas_Backend.Api.Models;

namespace MemoAtlas_Backend.Api.Models.Entities;

public class PromptAnswer : IPrivatable
{
    public int Id { get; set; }

    public int MemoId { get; set; }

    public int PromptId { get; set; }

    public string? TextValue { get; set; }

    public double? NumberValue { get; set; }

    public required bool Private { get; set; }

    // Navigation properties
    public Memo Memo { get; set; } = null!;
    public Prompt Prompt { get; set; } = null!;
}

