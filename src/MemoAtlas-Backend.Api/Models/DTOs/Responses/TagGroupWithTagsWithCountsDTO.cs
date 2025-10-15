namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagGroupWithTagsWithCountsDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required bool Private { get; set; }
    public required IEnumerable<TagWithCountAndWithoutGroupIdDTO> Tags { get; set; }
}