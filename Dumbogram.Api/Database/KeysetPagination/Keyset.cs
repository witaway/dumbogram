using System.Linq.Expressions;
using Dumbogram.Api.Database.KeysetPagination.Internals;
using Dumbogram.Api.Models.Base;

namespace Dumbogram.Api.Database.KeysetPagination;

public enum KeysetColumnOrder
{
    Ascending,
    Descending
}

public class Keyset<TEntity> where TEntity : BaseEntity
{
    public readonly List<IKeysetColumnOrder<TEntity>> Columns = new();

    public Keyset<TEntity> Ascending<TColumn>(
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

    public Keyset<TEntity> Descending<TColumn>(
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