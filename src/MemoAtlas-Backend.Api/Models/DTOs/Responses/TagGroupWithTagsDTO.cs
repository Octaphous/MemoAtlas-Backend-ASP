namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagGroupWithTagsDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required bool Private { get; set; }
    public required IEnumerable<TagWithoutGroupIdDTO> Tags { get; set; }
}