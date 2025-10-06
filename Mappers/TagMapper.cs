using MemoAtlas.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Mappers;

public static class TagMapper
{
    public static TagDTO ToDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        GroupId = tg.TagGroupId,
    };

    public static TagWithGroupAndMemosDTO ToTagWithGroupAndMemosDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        Group = TagGroupMapper.ToDTO(tg.TagGroup),
        Memos = tg.Memos.Select(MemoMapper.ToDTO).ToList(),
    };

    public static TagWithoutGroupDataDTO ToTagWithoutGroupDataDTO(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description
    };
}