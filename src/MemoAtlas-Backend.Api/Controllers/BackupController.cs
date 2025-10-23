using MemoAtlas_Backend.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using MemoAtlas_Backend.Api.Services.Interfaces;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using System.Text;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

namespace MemoAtlas_Backend.Api.Controllers;

[Route("api/backups")]
[AuthRequired]
[ApiController]
public class BackupController(IUserContext auth, IBackupService backupService, IBackupSigningService backupSigningService) : ControllerBase
{
    // A full backup is a json file containing all user data, that can be used to restore the data later.
    [HttpGet("full")]
    public async Task<IActionResult> FullBackup()
    {
        FullBackupV1 backup = await backupService.CreateFullBackupAsync(auth.GetRequiredUser());

        string json = backupSigningService.SerializeAndSign(backup);
        byte[] fileData = Encoding.UTF8.GetBytes(json);
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string fileName = $"memoatlas-backup-{timestamp}.json";
        string contentType = "application/json";

        return File(fileData, contentType, fileName);
    }

    // A readable backup is a backup more suitable for humans to read, meant for users leaving the platform. Not meant for restoring data.
    [HttpGet("readable")]
    public async Task<IActionResult> ReadableBackup()
    {
        return Ok();
    }

    [HttpPost("restore")]
    public async Task<IActionResult> RestoreBackup(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No backup file provided.");
        }

        using StreamReader reader = new(file.OpenReadStream());
        string content = await reader.ReadToEndAsync();

        IFullBackup backup = backupSigningService.DeserializeAndVerify(content);
        await backupService.RestoreFullBackupAsync(auth.GetRequiredUser(), backup);
        return Ok();
    }
}