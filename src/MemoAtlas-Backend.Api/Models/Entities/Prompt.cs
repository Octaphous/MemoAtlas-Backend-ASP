using System.ComponentModel.DataAnnotations;
using MemoAtlas_Backend.Api.Models;

namespace MemoAtlas_Backend.Api.Models.Entities;

public class Prompt : IPrivatable
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    [MaxLength(255)]
    public required string Question { get; set; }

    public required PromptType Type { get; set; }

    public required bool Private { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public List<PromptAnswer> PromptAnswers { get; set; } = [];
}

