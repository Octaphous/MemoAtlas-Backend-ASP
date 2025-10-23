using System.Text.Json.Serialization;

namespace MemoAtlas_Backend.Api.Models.DTOs.Backup.Interfaces;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "version")]
[JsonDerivedType(typeof(FullBackupV1), typeDiscriminator: "1")]
public interface IFullBackup { }