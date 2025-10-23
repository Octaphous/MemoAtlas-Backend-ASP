using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class FullBackupV1 : IFullBackup
{
    public required IEnumerable<MemoBackupV1> Memos { get; set; }
    public required IEnumerable<TagGroupBackupV1> TagGroups { get; set; }
    public required IEnumerable<TagBackupV1> Tags { get; set; }
    public required IEnumerable<MemoTagBackupV1> MemoTags { get; set; }
    public required IEnumerable<PromptBackupV1> Prompts { get; set; }
    public required IEnumerable<PromptAnswerTextBackupV1> PromptAnswerTexts { get; set; }
    public required IEnumerable<PromptAnswerNumberBackupV1> PromptAnswerNumbers { get; set; }
}