namespace MemoAtlas_Backend.Api.Models.DTOs.Backup;

public class MemoBackupV1
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required bool Private { get; set; }
}
