using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class SignedBackup
{
    public required string Signature { get; set; }
    public required IFullBackup Backup { get; set; }
}