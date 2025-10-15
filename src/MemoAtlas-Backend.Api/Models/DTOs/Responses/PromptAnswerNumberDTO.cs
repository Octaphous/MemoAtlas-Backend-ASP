namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class PromptAnswerNumberDTO : PromptAnswerDTO
{
    public required double Answer { get; set; }
}