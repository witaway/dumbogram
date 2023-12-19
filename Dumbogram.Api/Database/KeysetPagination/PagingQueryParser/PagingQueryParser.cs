using System.Runtime.CompilerServices;
using Dumbogram.Api.Database.KeysetPagination.Dto;
using Dumbogram.Api.Database.KeysetPagination.Enum;
using Dumbogram.Api.Models.Base;

namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser;

public class PagingQueryParser<TEntity> where TEntity : BaseEntity
{
    private readonly IPagingQueryKeysetParsingStrategy<TEntity> _getKeysetStrategy;
    private readonly int _takeLimit;
    
    public PagingQueryParser(IPagingQueryKeysetParsingStrategy<TEntity> getKeysetStrategy, int takeLimit = Int32.MaxValue)
    {
        _getKeysetStrategy = getKeysetStrategy;
        _takeLimit = takeLimit;
    }
    

    public PagingDetails<TEntity> GetPagingDetails(PagingQuery pagingQuery)
    {
        var keyset = GetKeyset(pagingQuery);
        var cursor = GetCursor(pagingQuery);
        var direction = GetDirection(pagingQuery);
        var take = GetTake(pagingQuery);
        
        return new PagingDetails<TEntity>(keyset, cursor, direction, take);
    }

    private Keyset<TEntity> GetKeyset(PagingQuery pagingQuery)
    {
        var order = pagingQuery.Order;

        return order is null
            ? _getKeysetStrategy.GetDefaultKeyset()
            : _getKeysetStrategy.GetKeyset(order);
    }
    
    private int GetTake(PagingQuery pagingQuery)
    {
        var take = pagingQuery.Take;
        
        return take == null
            ? _takeLimit
            : Math.Min(take.Value, _takeLimit);
    }
    
    private static PaginationDirection GetDirection(PagingQuery pagingQuery)
    {
        var isFirstRequested = pagingQuery.First;
        if (isFirstRequested)
        {
            return PaginationDirection.Forward;
        }

        var isLastRequested = pagingQuery.Last;
        if (isLastRequested)
        {
            return PaginationDirection.Backward;
        }
        
        var isNextRequested = pagingQuery.NextPageToken is not null;
        if (isNextRequested)
        {
            return PaginationDirection.Forward;
        }
        
        var isPreviousRequested = pagingQuery.PrevPageToken is not null;
        if (isPreviousRequested)
        {
            return PaginationDirection.Backward;
        }

        throw new SwitchExpressionException();
    }
    
    private Cursor<TEntity> GetCursor(PagingQuery pagingQuery)
    {
        // Getting data
        var first = pagingQuery.First;
        var last = pagingQuery.Last;
        var prevPageToken = pagingQuery.PrevPageToken;
        var nextPageToken = pagingQuery.NextPageToken;

        // Getting calculated data
        var take = GetTake(pagingQuery);
        var keyset = GetKeyset(pagingQuery);
        
        // If no specifiers defined - get first values
        var pagingPointNotSpecified = !first && !last && prevPageToken is null && nextPageToken is null;
        if (pagingPointNotSpecified)
        {
            pagingQuery.First = true;
        }

        // First or last
        if (first || last)
        {
            return first
                ? Cursor<TEntity>.First()
                : Cursor<TEntity>.Last();
        }
        
        // Previous or next token
        if (prevPageToken is not null || nextPageToken is not null)
        {
            var direction = GetDirection(pagingQuery);
            
            return Cursor<TEntity>.Decode(
                keyset, 
                prevPageToken!, 
                direction,
                take
            );
        }

        throw new SwitchExpressionException();
    }
}