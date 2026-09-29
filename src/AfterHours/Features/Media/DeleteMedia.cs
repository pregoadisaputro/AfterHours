using AfterHours.Data;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public sealed class DeleteMedia(IDbContextFactory<AppDbContext> db, ILogger<DeleteMedia> logger)
{
    public async Task HandleAsync(int id, CancellationToken ct = default)
    {
        using var dbCtx = await db.CreateDbContextAsync(ct);

        var deleteCount = await dbCtx.MediaItems.Where(m => m.Id == id).ExecuteDeleteAsync(ct);

        if (deleteCount == 0)
        {
            logger.LogWarning("Trying to deleting non-existing media with ID {MediaId}", id);
            return;
        }

        logger.LogInformation("Deleted {Count} media", deleteCount);
    }
}
