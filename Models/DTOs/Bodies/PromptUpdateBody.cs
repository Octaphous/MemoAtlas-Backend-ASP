using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class PromptUpdateBody
    {
        [MinLength(1, ErrorMessage = "Question cannot be empty.")]
        public string? Question { get; set; }
    }
}