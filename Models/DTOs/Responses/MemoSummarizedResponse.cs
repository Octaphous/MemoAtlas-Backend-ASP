using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class MemoSummarizedResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required int TagCount { get; set; }
    public required int PromptAnswerCount { get; set; }
}
