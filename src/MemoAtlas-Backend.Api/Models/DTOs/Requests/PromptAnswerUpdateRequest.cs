using System.Text.Json.Serialization;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PromptAnswerNumberUpdateRequest), (int)PromptType.Number)]
[JsonDerivedType(typeof(PromptAnswerTextUpdateRequest), (int)PromptType.Text)]
public abstract class PromptAnswerUpdateRequest(PromptType type)
{
    public PromptType Type { get; set; } = type;

    public int Id { get; set; }

    public bool? Private { get; set; }
}