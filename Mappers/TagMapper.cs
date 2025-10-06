using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

public static class TagMapper
{
    public static TagDetailedResponse ToDetailedResponse(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        GroupId = tg.TagGroupId,
        GroupName = tg.TagGroup.Name,
        GroupColor = tg.TagGroup.Color,
    };

    public static TagSummaryResponse ToSummarizedResponse(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description
    };
}