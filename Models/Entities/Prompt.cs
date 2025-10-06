using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.Entities;

public class Prompt
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    [MaxLength(255)]
    public required string Question { get; set; }

    public required PromptType Type { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public List<PromptAnswer> PromptAnswers { get; set; } = [];
}

