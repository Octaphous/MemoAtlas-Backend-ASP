using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

public static class MemoMapper
{
    public static MemoDTO ToDTO(Memo memo) => new MemoDTO
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        Private = memo.Private
    };

    public static MemoWithCountsDTO ToMemoWithCountsDTO(Memo memo) => new MemoWithCountsDTO
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        TagCount = memo.Tags.Count,
        PromptAnswerCount = memo.PromptAnswers.Count,
        Private = memo.Private
    };

    public static MemoWithTagsAndAnswersDTO ToMemoWithTagsAndAnswersDTO(Memo memo) => new()
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        TagGroups = memo.Tags
            .GroupBy(tag => tag.TagGroup)
            .Select(group => new TagGroupWithTagsDTO
            {
                Id = group.Key.Id,
                Name = group.Key.Name,
                Color = group.Key.Color,
                Tags = group.Select(tag => new TagWithoutGroupIdDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description,
                    Private = tag.Private,
                }),
                Private = group.Key.Private
            }),
        PromptAnswers = memo.PromptAnswers.Select(PromptAnswerMapper.ToPromptAnswerWithPromptDTO),
        Private = memo.Private
    };
}