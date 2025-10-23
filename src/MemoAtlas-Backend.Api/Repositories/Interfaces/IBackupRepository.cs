using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Repositories.Interfaces;

public interface IBackupRepository
{
    Task<FullBackupV1> CreateFullBackupAsync(User user);
    Task RestoreFullBackupAsync(User user, FullBackupV1 backup);
    // Task<FullBackupV2> UpgradeBackupV1ToV2Async(FullBackupV1 backupV1);
}
