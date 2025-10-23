using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class BackupService(IBackupRepository backupRepository) : IBackupService
{
    public async Task<FullBackupV1> CreateFullBackupAsync(User user)
    {
        FullBackupV1 backup = await backupRepository.CreateFullBackupAsync(user);
        return backup;
    }

    public async Task RestoreFullBackupAsync(User user, IFullBackup backup)
    {
        switch (backup)
        {
            case FullBackupV1 backupV1:
                await backupRepository.RestoreFullBackupAsync(user, backupV1);
                break;
            default:
                throw new InvalidPayloadException($"Backup version is not supported.");
        }
    }
}