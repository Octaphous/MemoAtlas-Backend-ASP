using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MemoAtlas_Backend_ASP.Converters;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class PromptAnswerUpdateRequest
{
    [Required]
    public required int Id { get; set; }

    [JsonConverter(typeof(JSONObjectConverter))]
    public object? Value { get; set; }

    public bool? Private { get; set; }
}
