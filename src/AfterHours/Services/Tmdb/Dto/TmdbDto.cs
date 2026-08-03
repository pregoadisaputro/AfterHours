using System.Text.Json.Serialization;

namespace AfterHours.Services.Tmdb.Dto;

public record TmdbMovieDetailsResponse(
    int Id,
    string? Title,
    string? Overview,
    int? Runtime,
    [property: JsonPropertyName("release_date")] DateOnly? ReleaseDate,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("backdrop_path")] string? BackdropPath
);

public record TmdbTvDetailsResponse(
    int Id,
    string? Name,
    string? Overview,
    int? Runtime,
    [property: JsonPropertyName("first_air_date")] DateOnly? FirstAirDate,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("backdrop_path")] string? BackdropPath
);

public record TmdbSearchResponse(
    int Page,
    [property: JsonPropertyName("total_pages")] int TotalPages,
    [property: JsonPropertyName("total_results")] int TotalResults,
    IReadOnlyList<TmdbSearchResults> Results
);

public record TmdbSearchResults(
    int Id,
    string? Title,
    string? Name,
    [property: JsonPropertyName("media_type")] string? MediaType,
    string? Overview,
    [property: JsonPropertyName("release_date")] DateOnly? ReleaseDate,
    [property: JsonPropertyName("first_air_date")] DateOnly? FirstAirDate,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("backdrop_path")] string? BackdropPath
);
