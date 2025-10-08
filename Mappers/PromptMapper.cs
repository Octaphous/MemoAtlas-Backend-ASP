using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Mappers;

public static class PromptMapper
{
    public static PromptDTO ToDTO(Prompt prompt) => new()
    {
        Id = prompt.Id,
        Question = prompt.Question,
        Type = prompt.Type,
        Private = prompt.Private
    };

    public static PromptWithMemosDTO ToPromptWithMemosDTO(Prompt prompt) => new()
    {
        Id = prompt.Id,
        Question = prompt.Question,
        Type = prompt.Type,
        Memos = prompt.PromptAnswers.Select(pa => new MemoWithPromptAnswerDTO()
        {
            Id = pa.Memo.Id,
            Title = pa.Memo.Title,
            Date = pa.Memo.Date,
            PromptAnswer = PromptAnswerMapper.ToDTO(pa),
            Private = pa.Memo.Private
        }),
        Private = prompt.Private
    };
}