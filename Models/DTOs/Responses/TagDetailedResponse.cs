using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class TagDetailedResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GroupId { get; set; }
    public required string GroupName { get; set; }
    public required string GroupColor { get; set; }
}
