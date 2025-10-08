namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagGroupDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required bool Private { get; set; }
}