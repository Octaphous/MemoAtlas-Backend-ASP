using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class TagResponse
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}