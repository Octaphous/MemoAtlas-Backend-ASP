namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GroupId { get; set; }
    public required bool Private { get; set; }
}