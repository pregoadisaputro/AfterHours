using AfterHours.Data;
using AfterHours.Data.Entity;
using AfterHours.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public record CreateMediaRequest(
    int ExternalId,
    decimal? Rating,
    string Title,
    MediaType MediaType,
    string? PosterPath,
    string? BackdropPath,
    DateOnly? ReleaseDate,
    MediaStatus MediaStatus
);

public sealed class CreateMedia(IDbContextFactory<AppDbContext> db, ILogger<CreateMedia> logger)
{
    public async Task HandleAsync(CreateMediaRequest request, CancellationToken ct = default)
    {
        using var dbCtx = await db.CreateDbContextAsync(ct);

        var existingMedia = await dbCtx
            .MediaItems.AsNoTracking()
            .AnyAsync(
                m => m.ExternalId == request.ExternalId && m.MediaType == request.MediaType,
                ct
            );

        if (existingMedia)
        {
            logger.LogWarning(
                "Media with ExternalId {ExternalId} and MediaType {MediaType} already exists.",
                request.ExternalId,
                request.MediaType
            );

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

        dbCtx.MediaItems.Add(newMedia);
        await dbCtx.SaveChangesAsync(ct);

        logger.LogInformation("Created media with ID {MediaId}", newMedia.Id);
    }
}
