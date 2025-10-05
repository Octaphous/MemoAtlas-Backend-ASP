using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class TagCreateBody
    {
        [Required, MinLength(1, ErrorMessage = "Name cannot be empty.")]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required int GroupId { get; set; }
    }
}