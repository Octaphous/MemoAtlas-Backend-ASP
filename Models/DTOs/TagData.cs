using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs;

public class TagData(Tag tag)
{
    public int Id { get; set; } = tag.Id;
    public string Name { get; set; } = tag.Name;
    public string Description { get; set; } = tag.Description;
    public int GroupId { get; set; } = tag.TagGroupId;
    public string GroupName { get; set; } = tag.TagGroup.Name;
    public string GroupColor { get; set; } = tag.TagGroup.Color;
}
