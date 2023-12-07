using Dumbogram.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Database.KeysetPagination;

public static class EFCoreExtension
{
    private static PaginationDirection GetDirection(PaginationDirection direction, CursorType cursorType)
    {
        if (cursorType == CursorType.Last)
        {
            direction = direction == PaginationDirection.Backward
                ? PaginationDirection.Forward
                : PaginationDirection.Backward;
        }

        return direction;
    }

    public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> query,
        Keyset<TEntity> keyset,
        Cursor<TEntity> cursor,
        CancellationToken token = default) where TEntity : BaseEntity
    {
        if (query == null)
        {
            throw new Exception();
        }

        if (keyset.Columns.Count == 0)
        {
            throw new Exception();
        }

        if (cursor.Take == 0)
        {
            throw new Exception();
        }

        var total = await query.CountAsync(token);
        var sortDirection = GetDirection(cursor.Direction, cursor.Type);

        var columns = keyset.Columns;

        var orderedQuery = columns[0]
            .ApplyOrderBy(query, sortDirection);

        for (var i = 1; i < columns.Count; i++)
        {
            orderedQuery = columns[1].ApplyThenOrderBy(orderedQuery, sortDirection);
        }

        var filteredQuery = orderedQuery.AsQueryable();
        if (cursor.Type == CursorType.Default)
        {
            var filter = ExpressionBuilder<TEntity>.BuildFilterExpression(keyset, cursor.Direction, cursor);
            filteredQuery = filteredQuery.Where(filter);
        }

        filteredQuery = filteredQuery.Take(cursor.Take + 1);

        var result = await filteredQuery.ToListAsync(token);

        if (result.Any())
        {
            var pagedResult = new PagedList<TEntity>(result.Slice(0, Math.Min(cursor.Take, result.Count)), total);

            if (cursor.Type == CursorType.Last || cursor.Direction == PaginationDirection.Backward)
            {
                pagedResult.Reverse();
            }

            pagedResult.Forward = Cursor<TEntity>.Encode(keyset, pagedResult.Last());
            if (cursor.Type == CursorType.Last || (
                    cursor.Direction == PaginationDirection.Forward &&
                    result.Count < cursor.Take + 1
                ))
            {
                pagedResult.Forward = null;
            }

            pagedResult.Backward = Cursor<TEntity>.Encode(keyset, pagedResult.First());
            if (cursor.Type == CursorType.First || (
                    cursor.Direction == PaginationDirection.Backward &&
                    result.Count < cursor.Take + 1
                ))
            {
                pagedResult.Backward = null;
            }

            return pagedResult;
        }

        return new PagedList<TEntity>(new List<TEntity>(), total);
    }
}