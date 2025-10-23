using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class TagBackupV1
{
    public required int Id { get; set; }
    public required int TagGroupId { get; set; }
    public required string Name { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public required bool Private { get; set; }
}