using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

public static class PromptAnswerMapper
{
    public static PromptAnswerResponse ToResponse(PromptAnswer pa) => new()
    {
        PromptId = pa.Prompt.Id,
        Type = pa.Prompt.Type,
        Question = pa.Prompt.Question,
        Value = pa.Prompt.Type switch
        {
            PromptType.Text => pa.TextValue,
            PromptType.Number => pa.NumberValue,
            _ => null
        }
    };
}