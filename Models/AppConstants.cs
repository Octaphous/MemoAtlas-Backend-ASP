namespace MemoAtlas_Backend_ASP.Models;

public static class AppConstants
{
    public static readonly string AuthTokenName = "SessionToken";

    public static readonly HashSet<string> AllowedTagColors = new(StringComparer.OrdinalIgnoreCase)
        {
            "red", "blue", "green", "yellow"
        };
}
