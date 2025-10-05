using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class PromptValueBody
    {
        [Required]
        public required int PromptId { get; set; }

        [Required]
        public required object Value { get; set; }
    }
}