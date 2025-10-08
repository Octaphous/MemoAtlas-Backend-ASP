using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MemoAtlas_Backend.Api.Converters;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class PromptAnswerCreateRequest
{
    [Required]
    public required int PromptId { get; set; }

    [Required]
    [JsonConverter(typeof(JSONObjectConverter))]
    public required object Value { get; set; }

    [Required]
    public required bool Private { get; set; }
}
