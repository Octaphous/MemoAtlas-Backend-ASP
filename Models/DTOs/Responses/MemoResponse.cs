using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class MemoResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required List<TagGroupResponse> TagGroups { get; set; }
    public required List<PromptAnswerResponse> PromptAnswers { get; set; }
}
