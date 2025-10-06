using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class PromptAnswerResponse(PromptAnswer pa)
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