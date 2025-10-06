using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class MemoResponse(Memo memo)
{
    public int Id { get; set; } = memo.Id;
    public string Title { get; set; } = memo.Title;
    public DateOnly Date { get; set; } = memo.Date;
    public List<TagDetailedResponse> Tags { get; set; } = memo.Tags.Select(tag => new TagDetailedResponse(tag)).ToList();
    public List<PromptAnswerResponse> PromptAnswers { get; set; } = memo.PromptAnswers.Select(pa => new PromptAnswerResponse(pa)).ToList();
}
