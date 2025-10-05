namespace MemoAtlas_Backend_ASP.Models.Entities
{
    public class Session
    {
        public int Id { get; set; }

        public required int UserId { get; set; }

        public required DateTime ExpiresAt { get; set; }

        public required string Token { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }
}