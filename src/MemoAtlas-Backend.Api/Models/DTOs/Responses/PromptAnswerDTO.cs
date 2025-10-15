using System.Text.Json.Serialization;

namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PromptAnswerNumberDTO), (int)PromptType.Number)]
[JsonDerivedType(typeof(PromptAnswerTextDTO), (int)PromptType.Text)]
public abstract class PromptAnswerDTO
{
    public required int Id { get; set; }
    public required bool Private { get; set; }
}