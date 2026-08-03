using AfterHours.Data;
using AfterHours.Data.Entity;
using AfterHours.Data.Enum;
using AfterHours.Features.Media.Dto;
using AfterHours.Services.Tmdb;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media.Services;

public sealed class MediaService(AppDbContext db, ILogger<MediaService> logger, TmdbService tmdb)
{
    private static DateOnly? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return DateOnly.TryParseExact(value, "yyyy-MM-dd", out var result) ? result : null;
    }

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
                ParseDate(movie.Release_Date),
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
            ParseDate(tv.First_Air_Date),
            tv.Runtime,
            MediaType.Tv,
            media?.MediaStatus,
            media != null
        );
    }

    public async Task<GetMediasPage> GetMediasAsync(
        GetMediasRequest request,
        CancellationToken ct = default
    )
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize switch
        {
            < 1 => 10,
            > 10 => 10,
            _ => request.PageSize,
        };

        var query = db.MediaItems.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Title}%"));
        }

        if (request.Rating.HasValue)
        {
            query = query.Where(x => x.Rating == request.Rating);
        }

        if (request.MediaType.HasValue)
        {
            query = query.Where(x => x.MediaType == request.MediaType);
        }

        if (request.MediaStatus.HasValue)
        {
            query = query.Where(x => x.MediaStatus == request.MediaStatus);
        }

        query = request.SortBy switch
        {
            MediaSortBy.RecentlyUpdated => query
                .OrderByDescending(m => m.UpdatedAt)
                .ThenBy(m => m.Id),
            _ => query.OrderByDescending(m => m.CreatedAt).ThenBy(m => m.Id),
        };

        var totalItems = await query.CountAsync(ct);
        var skip = (pageNumber - 1) * pageSize;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var response = await query
            .Skip(skip)
            .Take(pageSize)
            .Select(m => new GetMediasResponse(
                m.Id,
                m.ExternalId,
                m.Rating,
                m.Title,
                m.PosterPath,
                m.BackdropPath,
                m.ReleaseDate,
                m.MediaType,
                m.MediaStatus
            ))
            .ToListAsync(ct);

        return new GetMediasPage(pageNumber, pageSize, totalItems, totalPages, response);
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

    public async Task DeleteAsync(int id, CancellationToken ct = default)
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
