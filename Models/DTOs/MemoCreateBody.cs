using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class MemoCreateBody
    {
        [Required, MinLength(1, ErrorMessage = "Title cannot be empty.")]
        public required string Title { get; set; }

        [Required]
        public required DateOnly Date { get; set; }

        [MinLength(1, ErrorMessage = "At least one tag is required if tags are provided.")]
        public List<int>? Tags { get; set; }

        [MinLength(1, ErrorMessage = "At least one prompt is required if prompts are provided.")]
        public List<PromptValueBody>? Prompts { get; set; }
    }
}