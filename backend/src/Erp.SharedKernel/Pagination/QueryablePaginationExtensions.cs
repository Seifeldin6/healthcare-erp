using Microsoft.EntityFrameworkCore;

namespace Erp.SharedKernel.Pagination;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken ct = default)
    {
        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((request.NormalizedPageNumber - 1) * request.NormalizedPageSize)
            .Take(request.NormalizedPageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(items, totalCount, request.NormalizedPageNumber, request.NormalizedPageSize);
    }
}
