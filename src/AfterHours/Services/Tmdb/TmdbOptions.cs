namespace AfterHours.Services.Tmdb;

public class TmdbOptions
{
    public const string Name = "Tmdb";
    public required string ApiKey { get; set; }
    public string BaseUrl { get; set; } = "https://api.themoviedb.org/3/";
}
