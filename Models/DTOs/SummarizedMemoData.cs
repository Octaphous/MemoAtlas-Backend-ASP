using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs;

public class SummarizedMemoData
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int TagCount { get; set; }
    public int PromptCount { get; set; }

    /* These constructors are probably not needed for this class
    public SummarizedMemoData() { }

    public SummarizedMemoData(Memo memo)
    {
        Id = memo.Id;
        Title = memo.Title;
        Date = memo.Date;
        TagCount = memo.Tags.Count;
        PromptCount = memo.PromptAnswers.Count;
    }
    */
}
