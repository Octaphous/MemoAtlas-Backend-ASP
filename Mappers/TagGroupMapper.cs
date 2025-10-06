using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

public static class TagGroupMapper
{
    public static TagGroupResponse ToResponse(TagGroup tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Color = tg.Color,
        Tags = tg.Tags.Select(TagMapper.ToSummarizedResponse).ToList()
    };
}