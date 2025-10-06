namespace MemoAtlas_Backend_ASP.Models.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        public int TagGroupId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        // Navigation properties
        public TagGroup TagGroup { get; set; } = null!;
        public List<Memo> Memos { get; set; } = [];
    }
}
