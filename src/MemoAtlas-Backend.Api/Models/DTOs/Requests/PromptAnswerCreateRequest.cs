using System.Text.Json.Serialization;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PromptAnswerNumberCreateRequest), (int)PromptType.Number)]
[JsonDerivedType(typeof(PromptAnswerTextCreateRequest), (int)PromptType.Text)]
public abstract class PromptAnswerCreateRequest(PromptType type)
{
    public PromptType Type { get; set; } = type;

    public required int PromptId { get; set; }

    public required bool Private { get; set; }
}