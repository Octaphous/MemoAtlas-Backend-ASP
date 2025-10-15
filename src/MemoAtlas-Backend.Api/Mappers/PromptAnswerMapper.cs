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
            Text = pat.Text
        },
        PromptAnswerNumber pan => new PromptAnswerNumberDTO
        {
            Id = pan.Id,
            Private = pan.Private,
            Number = pan.Number
        },
        _ => throw new UnreachableException("Unknown PromptAnswer type")
    };
}