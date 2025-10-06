using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class SummarizedMemoData(Memo memo, int tagCount, int promptCount)
    {
        public int Id { get; set; } = memo.Id;
        public string Title { get; set; } = memo.Title;
        public DateOnly Date { get; set; } = memo.Date;
        public int TagCount { get; set; } = tagCount;
        public int PromptCount { get; set; } = promptCount;
    }
}