namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class TagWithGroupAndMemosDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool Private { get; set; }
    public required TagGroupDTO Group { get; set; }
    public required IEnumerable<MemoDTO> Memos { get; set; }
}
