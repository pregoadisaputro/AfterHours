using System.Globalization;
using AfterHours.Data;
using AfterHours.Data.Enum;
using AfterHours.Services.Tmdb;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

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

public sealed class GetMediaDetails(IDbContextFactory<AppDbContext> db, TmdbService tmdbService)
{
    public async Task<GetDetailsResponse?> GetAsync(
        MediaType mediaType,
        int externalId,
        CancellationToken ct = default
    )
    {
        using var dbCtx = await db.CreateDbContextAsync(ct);

        var media = await dbCtx
            .MediaItems.AsNoTracking()
            .Where(m => m.ExternalId == externalId && m.MediaType == mediaType)
            .FirstOrDefaultAsync(ct);

        if (mediaType == MediaType.Movie)
        {
            var movie = await tmdbService.GetMovieDetailsAsync(externalId, ct);

            if (movie is null)
            {
                return null;
            }

            return new GetDetailsResponse(
                media?.Id,
                externalId,
                media?.Rating,
                movie.Title,
                movie.Overview,
                movie.PosterPath,
                movie.BackdropPath,
                ParseDate(movie.ReleaseDate),
                movie.Runtime,
                MediaType.Movie,
                media?.MediaStatus,
                media != null
            );
        }

        var tv = await tmdbService.GetTvDetailsAsync(externalId, ct);

        if (tv is null)
        {
            return null;
        }

        return new GetDetailsResponse(
            media?.Id,
            externalId,
            media?.Rating,
            tv.Name,
            tv.Overview,
            tv.PosterPath,
            tv.BackdropPath,
            ParseDate(tv.FirstAirDate),
            tv.Runtime,
            MediaType.Tv,
            media?.MediaStatus,
            media != null
        );
    }

    private static DateOnly? ParseDate(string? value) =>
        DateOnly.TryParseExact(
            value,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var result
        )
            ? result
            : null;
}
