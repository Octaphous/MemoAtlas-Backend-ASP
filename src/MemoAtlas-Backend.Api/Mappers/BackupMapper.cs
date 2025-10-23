using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Mappers;

public static class BackupMapper
{
    public static MemoBackupV1 Memo(Memo memo) => new()
    {
        Id = memo.Id,
        Title = memo.Title,
        Date = memo.Date,
        Private = memo.Private
    };

    public static TagBackupV1 Tag(Tag tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Description = tg.Description,
        TagGroupId = tg.TagGroupId,
        Private = tg.Private
    };

    public static TagGroupBackupV1 TagGroup(TagGroup tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        Color = tg.Color,
        Private = tg.Private
    };

    public static PromptBackupV1 Prompt(Prompt prompt) => new()
    {
        Id = prompt.Id,
        Question = prompt.Question,
        Type = prompt.Type,
        Private = prompt.Private
    };

    public static PromptAnswerTextBackupV1 PromptAnswerText(PromptAnswerText pat) => new()
    {
        Id = pat.Id,
        MemoId = pat.MemoId,
        PromptId = pat.PromptId,
        Text = pat.Text,
        Private = pat.Private,
    };

    public static PromptAnswerNumberBackupV1 PromptAnswerNumber(PromptAnswerNumber pan) => new()
    {
        Id = pan.Id,
        MemoId = pan.MemoId,
        PromptId = pan.PromptId,
        Number = pan.Number,
        Private = pan.Private,
    };

}