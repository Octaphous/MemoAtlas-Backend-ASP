namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagWithCountAndWithoutGroupIdDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Count { get; set; }
    public required bool Private { get; set; }
}