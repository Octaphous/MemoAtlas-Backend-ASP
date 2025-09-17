using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MemoAtlas_Backend_ASP.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        // Navigation properties
        public List<Memo> Memos { get; set; } = [];
        public List<Prompt> Prompts { get; set; } = [];
        public List<TagGroup> TagGroups { get; set; } = [];
        public List<Session> Sessions { get; set; } = [];
    }
}
