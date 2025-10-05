using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class TagUpdateBody
    {
        [MinLength(1, ErrorMessage = "Name cannot be empty.")]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}