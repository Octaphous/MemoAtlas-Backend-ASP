using MemoAtlas_Backend.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Prompt> Prompts => Set<Prompt>();
    public DbSet<Memo> Memos => Set<Memo>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TagGroup> TagGroups => Set<TagGroup>();
    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<PromptAnswer> PromptAnswers => Set<PromptAnswer>();
    public DbSet<PromptAnswerText> PromptAnswerTexts => Set<PromptAnswerText>();
    public DbSet<PromptAnswerNumber> PromptAnswerNumbers => Set<PromptAnswerNumber>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Users can only store one memo for a given date
        mb.Entity<Memo>()
            .HasIndex(e => new { e.UserId, e.Date })
            .IsUnique();

        // Store subtypes of PromptAnswer in separate tables (Table-Per-Type)
        mb.Entity<PromptAnswer>()
            .UseTptMappingStrategy();
    }
}
