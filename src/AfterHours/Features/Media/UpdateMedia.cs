using AfterHours.Data;
using AfterHours.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public sealed record UpdateMediaRequest(decimal? Rating, MediaStatus MediaStatus);

public sealed class UpdateMedia(IDbContextFactory<AppDbContext> db, ILogger<UpdateMedia> logger)
{
    public async Task HandleAsync(
        int id,
        UpdateMediaRequest request,
        CancellationToken ct = default
    )
    {
        using var dbCtx = await db.CreateDbContextAsync(ct);

        logger.LogInformation("Updated Media with ID {MediaId}", id);

        var exsitingMedia =
            await dbCtx.MediaItems.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Media with ID {id} was not found.");

        exsitingMedia.Rating = request.Rating;
        exsitingMedia.MediaStatus = request.MediaStatus;
        exsitingMedia.UpdatedAt = DateTime.UtcNow;

        await dbCtx.SaveChangesAsync(ct);
    }
}
