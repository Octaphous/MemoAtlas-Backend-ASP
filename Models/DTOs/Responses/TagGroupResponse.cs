using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class TagGroupResponse(TagGroup tagGroup)
{
    public int Id { get; set; } = tagGroup.Id;
    public string Name { get; set; } = tagGroup.Name;
    public string Color { get; set; } = tagGroup.Color;
    public List<TagSummaryResponse> Tags { get; set; } = tagGroup.Tags.Select(t => new TagSummaryResponse(t)).ToList() ?? [];
}
