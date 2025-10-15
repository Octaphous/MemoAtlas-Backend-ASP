using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

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
        Memos = prompt.PromptAnswers.GroupBy(pa => pa.Memo).Select(g => new MemoWithPromptAnswerDTO
        {
            Id = g.Key.Id,
            Title = g.Key.Title,
            Date = g.Key.Date,
            Private = g.Key.Private,
            Answers = g.Select(PromptAnswerMapper.ToDTO)
        }),
        Private = prompt.Private
    };
}