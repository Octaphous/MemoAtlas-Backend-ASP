namespace MemoAtlas_Backend.Api.Models.Configurations;

public class AuthOptions
{
    public required int SessionDurationHours { get; set; }
    public required string SessionTokenName { get; set; }
}
