namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class PromptAnswerTextBackupV1 : PromptAnswerBackupV1
{
    public required string Text { get; set; }
}