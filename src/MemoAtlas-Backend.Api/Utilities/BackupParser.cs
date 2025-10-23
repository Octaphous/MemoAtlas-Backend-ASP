using System.Text.Json;
using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;

namespace MemoAtlas_Backend.Api.Utilities;

public static class BackupParser
{
    static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public static string Serialize(FullBackupV1 backup)
    {
        return JsonSerializer.Serialize(backup, options);
    }

    public static IFullBackup Deserialize(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("version", out JsonElement versionElement))
        {
            throw new InvalidPayloadException("Backup is missing 'version' property.");
        }

        string? version = versionElement.GetString();

        return version switch
        {
            "1" => DeserializeVersion<FullBackupV1>(json),
            _ => throw new InvalidPayloadException($"Backup version {version} is not supported.")
        };
    }

    private static T DeserializeVersion<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, options) ?? throw new InvalidPayloadException("Failed to deserialize backup.");
    }
}