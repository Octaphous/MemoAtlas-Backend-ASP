using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

public static class TagMapper
{
    public static TagDTO ToDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        GroupId = tg.TagGroupId,
        Private = tg.Private
    };

    public static TagWithGroupDTO ToTagWithGroupDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        Group = TagGroupMapper.ToDTO(tg.TagGroup),
        Private = tg.Private
    };

    public static TagWithoutGroupIdDTO ToTagWithoutGroupIdDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        Private = tg.Private
    };
}