using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Mappers;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class BackupService(IBackupRepository backupRepository, IMemoService memoService, IBackupFormatter backupFormatter) : IBackupService
{
    public async Task<string> CreateMarkdownBackupAsync(User user)
    {
        IEnumerable<Memo> memoEntities = await memoService.GetAllMemosAsync(user);
        IEnumerable<MemoWithTagsAndAnswersDTO> memoDTOs = memoEntities.Select(MemoMapper.ToMemoWithTagsAndAnswersDTO);
        return backupFormatter.ToMarkdown(memoDTOs);
    }

    public async Task<FullBackupV1> CreateFullBackupAsync(User user)
    {
        return await backupRepository.CreateFullBackupAsync(user);
    }

    public async Task RestoreFullBackupAsync(User user, IFullBackup backup)
    {
        if (!user.PrivateMode)
        {
            throw new UnauthenticatedException("For security reasons, restoring backups is only allowed when private mode is enabled.");
        }

        switch (backup)
        {
            case FullBackupV1 backupV1:
                await backupRepository.TryFullBackupRestoreAsync(user, backupV1);
                break;
            default:
                throw new InvalidPayloadException($"Backup version is not supported.");
        }
    }
}