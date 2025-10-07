using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class MemoTagUpdateRequest
{
    [MinLength(1, ErrorMessage = "No tags to add were provided.")]
    public IEnumerable<int>? Add { get; set; }

    [MinLength(1, ErrorMessage = "No tags to remove were provided.")]
    public IEnumerable<int>? Remove { get; set; }
}
