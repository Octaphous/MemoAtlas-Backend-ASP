using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class PromptData(Prompt prompt)
    {
        public int Id { get; set; } = prompt.Id;
        public string Question { get; set; } = prompt.Question;
        public int Type { get; set; } = (int)prompt.Type;
    }
}