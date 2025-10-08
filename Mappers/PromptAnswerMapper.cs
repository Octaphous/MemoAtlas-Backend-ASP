using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Mappers;

public static class PromptAnswerMapper
{
    public static PromptAnswerDTO ToDTO(PromptAnswer pa) => new()
    {
        Id = pa.Id,
        Value = pa.Prompt.Type switch
        {
            PromptType.Text => pa.TextValue,
            PromptType.Number => pa.NumberValue,
            _ => null
        },
        Private = pa.Private
    };

    public static PromptAnswerWithPromptDTO ToPromptAnswerWithPromptDTO(PromptAnswer pa) => new()
    {
        Id = pa.Id,
        Prompt = PromptMapper.ToDTO(pa.Prompt),
        Value = pa.Prompt.Type switch
        {
            PromptType.Text => pa.TextValue,
            PromptType.Number => pa.NumberValue,
            _ => null
        },
        Private = pa.Private
    };
}