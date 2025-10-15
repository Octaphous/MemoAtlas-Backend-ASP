using MemoAtlas_Backend.Api.Models.Entities.Interfaces;

namespace MemoAtlas_Backend.Api.Models.Entities;

public abstract class PromptAnswer : IPrivatable
{
    public int Id { get; set; }

    public int MemoId { get; set; }

    public int PromptId { get; set; }

    public required bool Private { get; set; }

    // Navigation properties
    public Memo Memo { get; set; } = null!;
    public Prompt Prompt { get; set; } = null!;
}

