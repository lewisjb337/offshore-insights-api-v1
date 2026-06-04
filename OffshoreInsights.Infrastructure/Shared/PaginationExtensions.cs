using Microsoft.EntityFrameworkCore;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Infrastructure.Shared;

public static class PaginationExtensions
{
    /// <summary>
    /// Executes a count and a paginated fetch against the provided query, returning
    /// the results wrapped in a <see cref="PagedResponse{T}"/> with full metadata.
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    /// <param name="query">The filtered, unordered <see cref="IQueryable{T}"/> to paginate.</param>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <param name="pageSize">The maximum number of items to return per page.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> containing the requested page of items and pagination metadata.</returns>
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var safePage     = Math.Max(1, page);
        var safePageSize = Math.Clamp(pageSize, 1, 200);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((safePage - 1) * safePageSize)
            .Take(safePageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<T>
        {
            Items = items,
            Page = safePage,
            PageSize = safePageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / safePageSize)
        };
    }
}
