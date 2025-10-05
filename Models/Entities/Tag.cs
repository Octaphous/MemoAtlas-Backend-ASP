namespace MemoAtlas_Backend_ASP.Models.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        public required int TagGroupId { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        // Navigation properties
        public TagGroup TagGroup { get; set; } = null!;
        public List<Memo> Memos { get; set; } = [];
    }
}
