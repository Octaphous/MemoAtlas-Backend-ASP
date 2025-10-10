using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

public static class TagGroupMapper
{
    public static TagGroupDTO ToDTO(TagGroup tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Color = tg.Color,
        Private = tg.Private
    };

    public static TagGroupWithTagsDTO ToTagGroupWithTagsDTO(TagGroup tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Color = tg.Color,
        Tags = tg.Tags.Select(TagMapper.ToTagWithoutGroupIdDTO),
        Private = tg.Private
    };
}