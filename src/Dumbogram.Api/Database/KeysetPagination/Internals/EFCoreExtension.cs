using Dumbogram.Api.Database.KeysetPagination.Dto;
using Dumbogram.Api.Database.KeysetPagination.Enum;
using Dumbogram.Api.Database.KeysetPagination.Internals.ExpressionBuilder;
using Dumbogram.Api.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Database.KeysetPagination.Internals;

public static class EFCoreExtension
{
    private static PaginationDirection GetSortDirection(PaginationDirection direction, CursorType cursorType)
    {
        if (cursorType == CursorType.Last)
        {
            direction = direction == PaginationDirection.Backward
                ? PaginationDirection.Forward
                : PaginationDirection.Backward;
        }

        return direction;
    }

    private static IQueryable<TEntity> ApplyKeysetSorting<TEntity>(
        this IQueryable<TEntity> query,
        Keyset<TEntity> keyset, 
        PaginationDirection sortDirection
        ) where TEntity : BaseEntity
    {
        var columns = keyset.Columns;

        var orderedQuery = columns.First().ApplyOrderBy(query, sortDirection);

        foreach (var column in columns.Skip(1))
        {
            orderedQuery = column.ApplyThenOrderBy(orderedQuery, sortDirection);
        }

        return orderedQuery.AsQueryable();
    }

    private static IQueryable<TEntity> ApplyKeysetFiltering<TEntity>(
        this IQueryable<TEntity> query,
        Keyset<TEntity> keyset,
        Cursor<TEntity> cursor,
        PaginationDirection direction
    ) where TEntity : BaseEntity
    {
        var filterExpression = ExpressionBuilder<TEntity>.BuildFilterExpression(
            keyset, 
            direction, 
            cursor
        );
        return query.Where(filterExpression);
    }

    public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> query,
        PagingDetails<TEntity> pagingDetails,
        CancellationToken token = default
    ) where TEntity : BaseEntity
    {
        var keyset = pagingDetails.Keyset;
        var cursor = pagingDetails.Cursor;
        var direction = pagingDetails.Direction;
        var take = pagingDetails.Take;
        
        return await ToPagedListAsync<TEntity>(
            query,
            keyset,
            cursor,
            direction,
            take,
            token
        );
    }
    
    private static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> query,
        Keyset<TEntity> keyset,
        Cursor<TEntity> cursor,
        PaginationDirection direction,
        int take,
        CancellationToken token = default
    ) where TEntity : BaseEntity
    {
        // =========================================
        // POINT 0
        // Initial checking up
        // =========================================
        //
        // We have troubles in one of cases:
        //
        //  1. Somehow query is null.
        //      It's not executable
        //
        //  2. Keyset does not contain no columns
        //      It's incorrect keyset and in fact it should not be like that      
        //
        //  3. Somehow we fetch 0 elements
        //      Incorrect case with undefined logic.

        if (query == null)
        {
            throw new Exception();
        }

        if (keyset.Columns.Count == 0)
        {
            throw new Exception();
        }

        if (take == 0)
        {
            throw new Exception();
        }
        
        var total = await query.CountAsync(token);

        // =========================================
        // POINT 1
        // Sort by elements from keyset
        // =========================================
        //
        // For example:
        //      new Keyset<Chat>()
        //          .Descending(m => m.CreationDate)
        //          .Ascending(m => m.Id)
        // Turns into:
        //      ORDER BY 
        //          creation_date DESC
        //          id            ASC
        //
        
        var actualSortDirection = GetSortDirection(direction, cursor.Type);
        query = query.ApplyKeysetSorting(keyset, actualSortDirection);
        
        // =========================================
        // POINT 2
        // Filter elements by cursor in accordance with keyset
        // =========================================
        // It takes cursor as a point in a database and ensures that only elements before/after this point is fetched
        // But if cursor is First or Last type we should not do any filtering
        // Because there's nothing to filter now
        
        if (cursor.Type is not CursorType.First && cursor.Type != CursorType.Last)
        {
            query = query.ApplyKeysetFiltering(keyset, cursor, direction);
        }

        
        // =========================================
        // POINT 3
        // Fetch elements from database
        // =========================================
        // Here we fetch one more element that requested
        // That's for purpose we could detect does next page exist or it's end
        
        query = query.Take(take + 1);
        var result = await query.ToListAsync(token);

        // =========================================
        // POINT 4
        // Pack fetched elements into our custom paged list
        // =========================================
        // It contains next page info et cetera
        // Later it could be parsed into tokens for frontend or used to fetch next page internally
        
        if (result.Any())
        {
            // POINT 4.1
            // Wrap result into PagedList
            // Note: before wrapping we skip extra elements (in fact, always 1 extra element) fetched for internal purposes
            //          (If it exist. Also it's possible that we on last page and ALL elements are important)
            
            var extraElements = Math.Max(0, result.Count - take);
            var pagedResult = new PagedList<TEntity>(result.SkipLast(extraElements), total);

            // POINT 4.2
            // Take edge case into account.
            // Previously, in case when we need backward page, we fetched elements in reverse order
            // Now we need reverse it back
            
            if (cursor.Type == CursorType.Last || direction == PaginationDirection.Backward)
            {
                pagedResult.Reverse();
            }

            // POINT 4.3
            // Save tokens for next and previous pages
            
            pagedResult.NextPageToken = Cursor<TEntity>.Encode(keyset, pagedResult.Last());
            pagedResult.PrevPageToken = Cursor<TEntity>.Encode(keyset, pagedResult.First());
            
            // POINT 4.4
            // Save whether we can go next/prev or not
            //
            // We cannot move next when out cursor type is Last
            //      or when we move forward and got less elements, than expected
            //
            // The check for moving backwards is same.
            
            var cannotMoveNext = cursor.Type == CursorType.Last || (
                direction == PaginationDirection.Forward &&
                result.Count < take + 1
            );
            pagedResult.CanMoveNext = !cannotMoveNext;

            var cannotMovePrev = cursor.Type == CursorType.First || (
                direction == PaginationDirection.Backward &&
                result.Count < take + 1
            );
            pagedResult.CanMovePrev = !cannotMovePrev;
            
            // POINT 4.5
            // Return PagedResult with initialized data
            return pagedResult;
        }

        // Case when we fetch nothing.
        return new PagedList<TEntity>(new List<TEntity>(), total);
    }
}