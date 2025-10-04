using System.ComponentModel.DataAnnotations;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class MemoData(Memo memo)
    {
        public int Id { get; set; } = memo.Id;

        public DateOnly Date { get; set; } = memo.Date;

        public List<MemoDataTag> Tags { get; set; } = [.. memo.Tags.Select(tag => new MemoDataTag(tag))];

        public List<MemoDataPrompt> Prompts { get; set; } = [.. memo.PromptAnswers.Select(pa => new MemoDataPrompt
        (pa))];
    }

    public class MemoDataTag(Tag tag)
    {
        public int Id { get; set; } = tag.Id;

        public string Name { get; set; } = tag.Name;

        public int TagGroupId { get; set; } = tag.TagGroupId;
    }

    public class MemoDataPrompt(PromptAnswer pa)
    {
        public int PromptId { get; set; } = pa.Prompt.Id;

        public string Question { get; set; } = pa.Prompt.Question;

        public PromptType Type { get; set; } = pa.Prompt.Type;

        public object? Answer { get; set; } = pa.Prompt.Type switch
        {
            PromptType.Text => pa.TextValue,
            PromptType.Number => pa.NumberValue,
            _ => null
        };
    }
}