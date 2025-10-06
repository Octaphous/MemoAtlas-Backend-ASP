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
        TagGroups = memo.Tags
            .GroupBy(tag => tag.TagGroup)
            .Select(group => new TagGroupResponse
            {
                Id = group.Key.Id,
                Name = group.Key.Name,
                Color = group.Key.Color,
                Tags = group.Select(tag => new TagSummaryResponse
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description
                }).ToList()
            }).ToList(),
        PromptAnswers = memo.PromptAnswers.Select(PromptAnswerMapper.ToResponse).ToList()
    };
}