using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class FullBackupV1 : IFullBackup
{
    public required List<MemoBackupV1> Memos { get; set; }
    public required List<TagGroupBackupV1> TagGroups { get; set; }
    public required List<TagBackupV1> Tags { get; set; }
    public required List<MemoTagBackupV1> MemoTags { get; set; }
    public required List<PromptBackupV1> Prompts { get; set; }
    public required List<PromptAnswerTextBackupV1> PromptAnswerTexts { get; set; }
    public required List<PromptAnswerNumberBackupV1> PromptAnswerNumbers { get; set; }
}