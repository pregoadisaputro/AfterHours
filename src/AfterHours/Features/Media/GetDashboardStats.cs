using AfterHours.Data;
using AfterHours.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Media;

public sealed record GetDashboardStatsReponse(MediaType MediaType, int TotalItems);

public sealed class GetDashboardStats(IDbContextFactory<AppDbContext> db)
{
    public async Task<IReadOnlyList<GetDashboardStatsReponse>> GetAsync(
        CancellationToken ct = default
    )
    {
        await using var dbCtx = await db.CreateDbContextAsync(ct);

        var response = await dbCtx
            .MediaItems.AsNoTracking()
            .GroupBy(m => m.MediaType)
            .Select(g => new GetDashboardStatsReponse(g.Key, g.Count()))
            .ToListAsync(ct);

        return response;
    }
}
