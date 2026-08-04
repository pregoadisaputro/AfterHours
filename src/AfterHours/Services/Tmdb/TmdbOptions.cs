namespace AfterHours.Services.Tmdb;

public sealed class TmdbOptions
{
    public const string Name = "Tmdb";
    public required string ApiKey { get; init; }
    public string BaseUrl { get; init; } = "https://api.themoviedb.org/3/";
}
