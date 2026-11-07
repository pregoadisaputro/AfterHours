using AfterHours.Data;
using AfterHours.Features.Shared;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public sealed class DeleteMedia(IDbContextFactory<AppDbContext> db, ILogger<DeleteMedia> logger)
{
    public async Task<Result> HandleAsync(int id, CancellationToken ct = default)
    {
        await using var dbCtx = await db.CreateDbContextAsync(ct);

        var deleteCount = await dbCtx.MediaItems.Where(m => m.Id == id).ExecuteDeleteAsync(ct);

        if (deleteCount == 0)
        {
            logger.LogWarning("Trying to deleting non-existing media with ID {MediaId}", id);
            return Result.Failure("Media was not found or empty.");
        }

        logger.LogInformation("Deleted media: {Count} & ID: {MediaId}", deleteCount, id);
        return Result.Success();
    }
}
