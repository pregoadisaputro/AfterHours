namespace AfterHours.Features.Media;

using AfterHours.Data;
using AfterHours.Data.Enum;
using Microsoft.EntityFrameworkCore;

public enum MediaSortBy
{
    RecentlyAdded,
    RecentlyUpdated,
}

public record GetMediasRequest(
    int PageNumber = 1,
    int PageSize = 24,
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
    string Title,
    string? PosterPath,
    MediaType MediaType,
    MediaStatus MediaStatus
);

public sealed class GetMedias(IDbContextFactory<AppDbContext> db)
{
    public async Task<GetMediasPage> GetAsync(
        GetMediasRequest request,
        CancellationToken ct = default
    )
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize switch
        {
            < 1 => 24,
            > 100 => 100,
            _ => request.PageSize,
        };

        using var dbCtx = await db.CreateDbContextAsync(ct);

        var query = dbCtx.MediaItems.AsNoTracking().AsQueryable();

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
                m.MediaType,
                m.MediaStatus
            ))
            .ToListAsync(ct);

        return new GetMediasPage(pageNumber, pageSize, totalItems, totalPages, response);
    }
}
