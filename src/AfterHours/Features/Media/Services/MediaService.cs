using AfterHours.Data;
using AfterHours.Data.Entity;
using AfterHours.Data.Enum;
using AfterHours.Features.Media.Dto;
using AfterHours.Services.Tmdb;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media.Services;

public sealed class MediaService(AppDbContext db, ILogger<MediaService> logger, ITmdbService tmdb)
    : IMediaService
{
    public async Task<GetDetailsResponse?> GetDetailsAsync(
        MediaType mediaType,
        int externalId,
        CancellationToken ct = default
    )
    {
        var media = await db
            .MediaItems.AsNoTracking()
            .FirstOrDefaultAsync(m => m.ExternalId == externalId && m.MediaType == mediaType, ct);

        if (mediaType == MediaType.Movie)
        {
            var movie = await tmdb.GetMovieDetailsAsync(externalId, ct);

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
                movie.Poster_Path,
                movie.Backdrop_Path,
                movie.ReleaseDate,
                movie.Runtime,
                MediaType.Movie,
                media?.MediaStatus,
                media != null
            );
        }

        var tv = await tmdb.GetTvDetailsAsync(externalId, ct);

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
            tv.Poster_Path,
            tv.Backdrop_Path,
            tv.FirstAirDate,
            tv.Runtime,
            MediaType.Tv,
            media?.MediaStatus,
            media != null
        );
    }

    public async Task CreateAsync(CreateMediaRequest request, CancellationToken ct = default)
    {
        var existingMedia = await db
            .MediaItems.AsNoTracking()
            .AnyAsync(
                m => m.ExternalId == request.ExternalId && m.MediaType == request.MediaType,
                ct
            );

        if (existingMedia)
        {
            return;
        }

        var newMedia = new MediaItem
        {
            ExternalId = request.ExternalId,
            Rating = request.Rating,
            Title = request.Title,
            MediaType = request.MediaType,
            PosterPath = request.PosterPath,
            BackdropPath = request.BackdropPath,
            ReleaseDate = request.ReleaseDate,
            MediaStatus = request.MediaStatus,
        };

        db.MediaItems.Add(newMedia);
        await db.SaveChangesAsync(ct);

        logger.LogInformation("Created media with ID {MediaId}", newMedia.Id);
    }

    public async Task UpdateAsync(
        int id,
        UpdateMediaRequest request,
        CancellationToken ct = default
    )
    {
        var media = await db.MediaItems.FindAsync([id], ct);

        if (media is null)
        {
            return;
        }

        media.Rating = request.Rating;
        media.MediaStatus = request.Status;
        media.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        logger.LogInformation("Updated media with ID {MediaId}", id);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var deletedMedia = await db.MediaItems.FindAsync([id], ct);

        if (deletedMedia is null)
        {
            return;
        }

        db.MediaItems.Remove(deletedMedia);
        await db.SaveChangesAsync(ct);

        logger.LogInformation("Deleted media with ID {MediaId}", id);
    }
}
