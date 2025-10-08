using MemoAtlas.Models;

namespace MemoAtlas_Backend_ASP.Models.Entities;

public class Tag : IPrivatable
{
    public int Id { get; set; }

    public int TagGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public required bool Private { get; set; }

    // Navigation properties
    public TagGroup TagGroup { get; set; } = null!;
    public List<Memo> Memos { get; set; } = [];
}