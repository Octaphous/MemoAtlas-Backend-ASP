namespace MemoAtlas_Backend_ASP.Models.Entities
{
    public class Memo
    {
        public int Id { get; set; }

        public required int UserId { get; set; }

        public required string Title { get; set; }

        public required DateOnly Date { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public List<PromptAnswer> PromptAnswers { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
    }
}
