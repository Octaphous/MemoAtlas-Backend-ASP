using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs;

public class MemoData(Memo memo)
{
    public int Id { get; set; } = memo.Id;
    public string Title { get; set; } = memo.Title;
    public DateOnly Date { get; set; } = memo.Date;
    public List<TagDetails> Tags { get; set; } = [.. memo.Tags.Select(tag => new TagDetails(tag))];
    public List<PromptDetails> Prompts { get; set; } = [.. memo.PromptAnswers.Select(pa => new PromptDetails(pa))];

    public class TagDetails(Tag tag)
    {
        public int Id { get; set; } = tag.Id;
        public string Name { get; set; } = tag.Name;
        public int TagGroupId { get; set; } = tag.TagGroupId;
    }

    public class PromptDetails(PromptAnswer pa)
    {
        public int PromptId { get; set; } = pa.Prompt.Id;
        public PromptType Type { get; set; } = pa.Prompt.Type;
        public string Question { get; set; } = pa.Prompt.Question;
        public object? Value { get; set; } = pa.Prompt.Type switch
        {
            PromptType.Text => pa.TextValue,
            PromptType.Number => pa.NumberValue,
            _ => null
        };
    }
}
