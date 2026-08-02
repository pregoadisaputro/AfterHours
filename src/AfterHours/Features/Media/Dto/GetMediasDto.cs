using AfterHours.Data.Enum;

namespace AfterHours.Features.Media.Dto;

public enum MediaSortBy
{
    RecentlyAdded,
    RecentlyUpdated,
    Id,
}

public record GetMediasRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? Title = null,
    decimal? Rating = null,
    MediaType? MediaType = null,
    MediaStatus? MediaStatus = null,
    MediaSortBy SortBy = MediaSortBy.RecentlyAdded
);

public record GetMediasPage(
    int PageNumber,
    int PageSize,
    int TotalItems,
    int TotalPages,
    IReadOnlyList<GetMediasResponse> Data
);

public record GetMediasResponse(
    int Id,
    int ExternalId,
    decimal? Rating,
    string? Title,
    string? PosterPath,
    string? BackdropPath,
    DateOnly? ReleaseDate,
    MediaType MediaType,
    MediaStatus MediaStatus
);
