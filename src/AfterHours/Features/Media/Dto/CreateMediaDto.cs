using AfterHours.Data.Enum;

namespace AfterHours.Features.Media.Dto;

public record CreateMediaRequest(
    int ExternalId,
    decimal? Rating,
    string? Title,
    MediaType MediaType,
    string? PosterPath,
    string? BackdropPath,
    DateOnly? ReleaseDate,
    MediaStatus MediaStatus
);
