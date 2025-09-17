using MemoAtlas_Backend_ASP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Prompt> Prompts => Set<Prompt>();
        public DbSet<Memo> Memos => Set<Memo>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TagGroup> TagGroups => Set<TagGroup>();
        public DbSet<PromptAnswer> PromptAnswers => Set<PromptAnswer>();
        public DbSet<Session> Sessions => Set<Session>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.Entity<Memo>().HasIndex(e => new { e.UserId, e.Date }).IsUnique();
        }
    }
}
