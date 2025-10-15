using System.Diagnostics;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

public static class PromptAnswerMapper
{
    public static PromptAnswerDTO ToDTO(PromptAnswer pa) => pa switch
    {
        PromptAnswerText pat => new PromptAnswerTextDTO
        {
            Id = pat.Id,
            Private = pat.Private,
            Answer = pat.Answer
        },
        PromptAnswerNumber pan => new PromptAnswerNumberDTO
        {
            Id = pan.Id,
            Private = pan.Private,
            Answer = pan.Answer
        },
        _ => throw new UnreachableException("Unknown PromptAnswer type")
    };

    public static PromptAnswerWithPromptDTO ToPromptAnswerWithPromptDTO(PromptAnswer pa) => new()
    {
        Data = ToDTO(pa),
        Prompt = PromptMapper.ToDTO(pa.Prompt)
    };
}