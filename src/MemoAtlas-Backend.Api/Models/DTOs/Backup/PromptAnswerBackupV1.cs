namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public abstract class PromptAnswerBackupV1
{
    public required int Id { get; set; }
    public required int MemoId { get; set; }
    public required int PromptId { get; set; }
    public required bool Private { get; set; }
}