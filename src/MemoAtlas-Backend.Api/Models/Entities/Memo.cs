using MemoAtlas_Backend.Api.Models;

namespace MemoAtlas_Backend.Api.Models.Entities;

public class Memo : IPrivatable
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public required string Title { get; set; }

    public required DateOnly Date { get; set; }

    public required bool Private { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public List<PromptAnswer> PromptAnswers { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}
