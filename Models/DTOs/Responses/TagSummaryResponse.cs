using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class TagSummaryResponse(Tag tag)
{
    public int Id { get; set; } = tag.Id;
    public string Name { get; set; } = tag.Name;
    public string Description { get; set; } = tag.Description;
}