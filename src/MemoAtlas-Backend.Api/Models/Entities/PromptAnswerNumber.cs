namespace MemoAtlas_Backend.Api.Models.Entities;

public class PromptAnswerNumber : PromptAnswer
{
    public required double Answer { get; set; }
}