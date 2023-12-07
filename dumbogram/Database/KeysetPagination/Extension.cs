using Dumbogram.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Database.KeysetPagination;

public static class Extension
{
    private static KeysetPaginationDirection GetDirection(KeysetPaginationDirection direction, CursorType cursorType)
    {
        if (cursorType == CursorType.Last)
        {
            direction = direction == KeysetPaginationDirection.Backward
                ? KeysetPaginationDirection.Forward
                : KeysetPaginationDirection.Backward;
        }

        return direction;
    }

    public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> query,
        KeysetOrder<TEntity> keysetOrder,
        Cursor<TEntity> cursor,
        CancellationToken token = default) where TEntity : BaseEntity
    {
        if (query == null)
        {
            throw new Exception();
        }

        if (keysetOrder.Columns.Count == 0)
        {
            throw new Exception();
        }

        if (cursor.Take == 0)
        {
            throw new Exception();
        }

        var total = await query.CountAsync(token);
        var sortDirection = GetDirection(cursor.Direction, cursor.Type);

        var columns = keysetOrder.Columns;

        var orderedQuery = columns[0]
            .ApplyOrderBy(query, sortDirection);

        for (var i = 1; i < columns.Count; i++)
        {
            orderedQuery = columns[1].ApplyThenOrderBy(orderedQuery, sortDirection);
        }

        var filteredQuery = orderedQuery.AsQueryable();
        if (cursor.Type == CursorType.Default)
        {
            var filter = CBuildExpression<TEntity>.BuildExpression(keysetOrder, cursor.Direction, cursor);
            filteredQuery = filteredQuery.Where(filter);
        }

        filteredQuery = filteredQuery.Take(cursor.Take + 1);

        var result = await filteredQuery.ToListAsync(token);

        if (result.Any())
        {
            var pagedResult = new PagedList<TEntity>(result.Slice(0, Math.Min(cursor.Take, result.Count)), total);

            if (cursor.Type == CursorType.Last || cursor.Direction == KeysetPaginationDirection.Backward)
            {
                pagedResult.Reverse();
            }

            pagedResult.Forward = Cursor<TEntity>.Encode(keysetOrder, pagedResult.Last());
            if (cursor.Type == CursorType.Last || (
                    cursor.Direction == KeysetPaginationDirection.Forward &&
                    result.Count < cursor.Take + 1
                ))
            {
                pagedResult.Forward = null;
            }

            pagedResult.Backward = Cursor<TEntity>.Encode(keysetOrder, pagedResult.First());
            if (cursor.Type == CursorType.First || (
                    cursor.Direction == KeysetPaginationDirection.Backward &&
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