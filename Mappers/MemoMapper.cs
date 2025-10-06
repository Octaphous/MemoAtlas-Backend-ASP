using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Mappers;

public static class MemoMapper
{
    public static MemoSummarizedResponse ToSummarizedResponse(Memo memo) => new MemoSummarizedResponse
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        TagCount = memo.Tags.Count,
        PromptAnswerCount = memo.PromptAnswers.Count
    };

    public static MemoResponse ToResponse(Memo memo) => new()
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        Tags = memo.Tags.Select(TagMapper.ToDetailedResponse).ToList(),
        PromptAnswers = memo.PromptAnswers.Select(PromptAnswerMapper.ToResponse).ToList()
    };
}