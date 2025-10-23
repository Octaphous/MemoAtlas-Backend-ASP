using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IBackupSigningService
{
    SignedBackup SignBackup(IFullBackup backup);
    bool VerifySignedBackup(SignedBackup signedBackup);
    string SerializeAndSign(IFullBackup backup);
    IFullBackup DeserializeAndVerify(string json);
}
