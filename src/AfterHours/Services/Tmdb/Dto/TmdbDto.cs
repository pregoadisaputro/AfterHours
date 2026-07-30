using System.Text.Json.Serialization;

namespace AfterHours.Services.Tmdb.Dto;

public record TmdbMovieDetailsResponse(
    int Id,
    string? Title,
    string? Overview,
    int? Runtime,
    [property: JsonPropertyName("release_date")] DateOnly? ReleaseDate,
    string? Poster_Path,
    string? Backdrop_Path
);

public record TmdbTvDetailsResponse(
    int Id,
    string? Name,
    string? Overview,
    int? Runtime,
    [property: JsonPropertyName("first_air_date")] DateOnly? FirstAirDate,
    string? Poster_Path,
    string? Backdrop_Path
);

public record TmdbSearchResponse(
    int Page,
    int Total_Pages,
    int TotalResutls,
    IReadOnlyList<TmdbSearchResults> Results
);

public record TmdbSearchResults(
    int Id,
    string? Title,
    string? Name,
    string? Media_Type,
    string? Overview,
    [property: JsonPropertyName("release_date")] DateOnly? ReleaseDate,
    [property: JsonPropertyName("first_air_date")] DateOnly? FirstAirDate,
    string? Poster_Path,
    string? Backdrop_Path
);
