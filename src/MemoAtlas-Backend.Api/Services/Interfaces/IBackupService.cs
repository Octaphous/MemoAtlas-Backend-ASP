using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IBackupService
{
    Task<string> CreateMarkdownBackupAsync(User user);
    Task<FullBackupV1> CreateFullBackupAsync(User user);
    Task RestoreFullBackupAsync(User user, IFullBackup backup);
    // Task<FullBackupV2> UpgradeBackupV1ToV2Async(FullBackupV1 backupV1);
}
