namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class TagGroupWithTagsDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required IEnumerable<TagWithoutGroupDataDTO> Tags { get; set; }
}