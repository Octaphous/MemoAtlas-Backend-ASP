namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class TagGroupBackupV1
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required bool Private { get; set; }
}