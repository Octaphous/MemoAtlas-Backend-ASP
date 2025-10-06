using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

public static class PromptMapper
{
    public static PromptResponse ToResponse(Prompt prompt) => new()
    {
        Id = prompt.Id,
        Question = prompt.Question,
        Type = prompt.Type
    };
}

