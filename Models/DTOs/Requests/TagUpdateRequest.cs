using System.ComponentModel.DataAnnotations;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class TagUpdateRequest
{
    [MinLength(1, ErrorMessage = "Name cannot be empty.")]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Private { get; set; }
}