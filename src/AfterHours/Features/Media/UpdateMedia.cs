using AfterHours.Data;
using AfterHours.Data.Enum;
using AfterHours.Features.Shared;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public sealed record UpdateMediaRequest(decimal? Rating, MediaStatus MediaStatus);

public sealed class UpdateMedia(IDbContextFactory<AppDbContext> db, ILogger<UpdateMedia> logger)
{
    public async Task<Result> HandleAsync(
        int id,
        UpdateMediaRequest request,
        CancellationToken ct = default
    )
    {
        await using var dbCtx = await db.CreateDbContextAsync(ct);

        var exsitingMedia = await dbCtx.MediaItems.FindAsync([id], ct);

        if (exsitingMedia is null)
        {
            logger.LogWarning("Media is null, ID: {MediaID}", id);
            return Result.Failure("Media was not found or empty.");
        }

        exsitingMedia.Rating = request.Rating;
        exsitingMedia.MediaStatus = request.MediaStatus;
        exsitingMedia.UpdatedAt = DateTime.UtcNow;

        try
        {
            await dbCtx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error occurred while updating media, ID: {MediaID}", id);
            return Result.Failure("Unable to update media, Please try again.");
        }

        logger.LogInformation("Updated Media with ID {MediaId}", id);

        return Result.Success();
    }
}
