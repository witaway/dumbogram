using System.Linq.Expressions;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public interface IKeysetColumnOrder<TEntity>
    : IKeysetColumn<TEntity>
    where TEntity : BaseEntity
{
    public KeysetColumnOrder Order { get; set; }

    public IOrderedQueryable<TEntity> ApplyOrderBy(
        IQueryable<TEntity> query,
        KeysetPaginationDirection direction
    );

    public IOrderedQueryable<TEntity> ApplyThenOrderBy(
        IOrderedQueryable<TEntity> query,
        KeysetPaginationDirection direction
    );
}

public class KeysetColumnOrder<TEntity, TColumn> : KeysetColumn<TEntity, TColumn>, IKeysetColumnOrder<TEntity>
    where TEntity : BaseEntity
    where TColumn : IComparable
{
    public KeysetColumnOrder(
        Expression<Func<TEntity, TColumn>> propertySelectorExpression,
        KeysetColumnOrder order,
        string? name = null
    )
        : base(propertySelectorExpression, name)
    {
        Order = order;
    }

    public KeysetColumnOrder Order { get; set; }

    public IOrderedQueryable<TEntity> ApplyOrderBy(
        IQueryable<TEntity> query,
        KeysetPaginationDirection direction
    )
    {
        return ApplyOrderByVariant(
            query, direction,
            Queryable.OrderBy,
            Queryable.OrderByDescending);
    }

    public IOrderedQueryable<TEntity> ApplyThenOrderBy(
        IOrderedQueryable<TEntity> query,
        KeysetPaginationDirection direction
    )
    {
        return ApplyOrderByVariant(
            query, direction,
            Queryable.ThenBy,
            Queryable.ThenByDescending);
    }

    private IOrderedQueryable<TEntity> ApplyOrderByVariant<TQueryable>(
        TQueryable query,
        KeysetPaginationDirection direction,
        Func<TQueryable, Expression<Func<TEntity, TColumn>>, IOrderedQueryable<TEntity>> ascendingVariant,
        Func<TQueryable, Expression<Func<TEntity, TColumn>>, IOrderedQueryable<TEntity>> descendingVariant)
        where TQueryable : IQueryable<TEntity>
    {
        var isDescending = direction == KeysetPaginationDirection.Backward
            ? Order == KeysetColumnOrder.Descending
            : Order == KeysetColumnOrder.Ascending;

        return isDescending
            ? descendingVariant(query, PropertySelectorExpression)
            : ascendingVariant(query, PropertySelectorExpression);
    }
}