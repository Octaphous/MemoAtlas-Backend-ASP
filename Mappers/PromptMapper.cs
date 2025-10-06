using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Mappers;

public static class PromptMapper
{
    public static PromptDTO ToDTO(Prompt prompt) => new()
    {
        Id = prompt.Id,
        Question = prompt.Question,
        Type = prompt.Type
    };
}

