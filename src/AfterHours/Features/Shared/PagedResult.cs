using Microsoft.EntityFrameworkCore;

namespace AfterHours.Features.Shared;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalItems,
    int PageNumber,
    int PageSize,
    int TotalPages
)
{
    public static async Task<PagedResult<T>> CreateAsync(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var normalizedPage = pageNumber < 1 ? 1 : pageNumber;
        var normalizedSize = pageSize switch
        {
            < 1 => 36,
            > 100 => 100,
            _ => pageSize,
        };

        var totalItems = await query.CountAsync(ct);
        var skipCount = (normalizedPage - 1) * normalizedSize;
        var totalPages = (int)Math.Ceiling((double)totalItems / normalizedSize);

        var items = await query.Skip(skipCount).Take(normalizedSize).ToListAsync(ct);

        return new PagedResult<T>(items, totalItems, normalizedPage, normalizedSize, totalPages);
    }
}
