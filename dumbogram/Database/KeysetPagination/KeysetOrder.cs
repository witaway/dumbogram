using System.Linq.Expressions;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public enum KeysetColumnOrder
{
    Ascending,
    Descending
}

public enum KeysetPaginationDirection
{
    Forward,
    Backward
}

public class KeysetOrder<TEntity> where TEntity : BaseEntity
{
    public readonly List<IKeysetColumnOrder<TEntity>> Columns = new();

    public KeysetOrder<TEntity> Ascending<TColumn>(
        Expression<Func<TEntity, TColumn>> propertySelector,
        string? name = null
    )
        where TColumn : IComparable
    {
        Columns.Add(new KeysetColumnOrder<TEntity, TColumn>(
            propertySelector,
            KeysetColumnOrder.Descending,
            name
        ));
        return this;
    }

    public KeysetOrder<TEntity> Descending<TColumn>(
        Expression<Func<TEntity, TColumn>> propertySelector,
        string? name = null
    )
        where TColumn : IComparable
    {
        Columns.Add(new KeysetColumnOrder<TEntity, TColumn>(
            propertySelector,
            KeysetColumnOrder.Ascending,
            name
        ));
        return this;
    }
}