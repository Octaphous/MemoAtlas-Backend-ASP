using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.Configurations;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoAtlas_Backend.Api.Services;

public class BackupSigningService(IOptions<SecurityOptions> options) : IBackupSigningService
{
    private readonly byte[] signingKey = Encoding.UTF8.GetBytes(options.Value.BackupSigningKey);
    static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public SignedBackup SignBackup(IFullBackup backup)
    {
        string json = JsonSerializer.Serialize(backup, backup.GetType(), options);

        using HMACSHA256 hmac = new(signingKey);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(json));

        return new SignedBackup
        {
            Signature = Convert.ToBase64String(hash),
            Backup = backup
        };
    }

    public bool VerifySignedBackup(SignedBackup signedBackup)
    {
        if (string.IsNullOrEmpty(signedBackup.Signature))
            return false;

        string expectedSignature = SignBackup(signedBackup.Backup).Signature;
        return expectedSignature == signedBackup.Signature;
    }

    public string SerializeAndSign(IFullBackup backup)
    {
        SignedBackup signedBackup = SignBackup(backup);
        return JsonSerializer.Serialize(signedBackup, options);
    }

    public IFullBackup DeserializeAndVerify(string json)
    {
        SignedBackup? signedBackup = JsonSerializer.Deserialize<SignedBackup>(json, options)
            ?? throw new InvalidPayloadException("Backup format is invalid.");

        if (!VerifySignedBackup(signedBackup))
            throw new InvalidPayloadException("Invalid backup signature.");

        return signedBackup.Backup;
    }
}