using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MemoAtlas_Backend_ASP.Converters;

namespace MemoAtlas_Backend_ASP.Models.DTOs.Requests;

public class PromptAnswerRequest
{
    [Required]
    public required int PromptId { get; set; }

    [Required]
    [JsonConverter(typeof(JSONObjectConverter))]
    public required object Value { get; set; }
}
