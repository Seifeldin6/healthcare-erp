using Microsoft.EntityFrameworkCore;

namespace Erp.SharedKernel.Pagination;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken ct = default)
    {
        var pageNumber = request.NormalizedPageNumber < 1 ? 1 : request.NormalizedPageNumber;
        var pageSize = Math.Clamp(request.NormalizedPageSize, 1, PagedRequest.MaxPageSize);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(items, totalCount, pageNumber, pageSize);
    }
}
