using MemoAtlas_Backend.Api.Models.Entities.Interfaces;

namespace MemoAtlas_Backend.Api.Models.Entities;

public class TagGroup : IPrivatable
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public required string Name { get; set; }

    public required string Color { get; set; }

    public required bool Private { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public List<Tag> Tags { get; set; } = [];
}
