using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class TagSummaryResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}