using AfterHours.Data.Enum;

namespace AfterHours.Features.Media.Dto;

public record GetDetailsResponse(
    int? Id,
    int ExternalId,
    decimal? Rating,
    string? Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    DateOnly? ReleaseDate,
    int? Runtime,
    MediaType MediaType,
    MediaStatus? MediaStatus,
    bool IsSaved
);
