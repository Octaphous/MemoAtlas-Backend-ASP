using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptResponse(Prompt prompt)
{
    public int Id { get; set; } = prompt.Id;
    public string Question { get; set; } = prompt.Question;
    public PromptType Type { get; set; } = prompt.Type;
}
