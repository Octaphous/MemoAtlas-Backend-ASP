namespace MemoAtlas_Backend_ASP.Models.Entities;

public class TagGroup
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public required string Name { get; set; }

    public required string Color { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public List<Tag> Tags { get; set; } = [];
}
