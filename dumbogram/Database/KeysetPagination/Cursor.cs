using System.Linq.Expressions;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public enum CursorType
{
    Default,
    First,
    Last
}

public partial class Cursor<TEntity> where TEntity : BaseEntity
{
    public readonly List<IKeysetColumnValue<TEntity>> Values = new();
    public PaginationDirection Direction;
    public Keyset<TEntity> Keyset;
    public int Take;
    public CursorType Type;


    public Cursor(Keyset<TEntity> keyset, PaginationDirection direction, int take)
    {
        Keyset = keyset;
        Direction = direction;
        Take = take;
        Type = CursorType.Default;
    }

    protected Cursor(CursorType type)
    {
        Type = type;
    }

    public static Cursor<TEntity> First(int take)
    {
        return new Cursor<TEntity>(CursorType.First) { Take = take };
    }

    public static Cursor<TEntity> Last(int take)
    {
        return new Cursor<TEntity>(CursorType.Last) { Take = take };
    }

    private Cursor<TEntity> ColumnValue<TColumn>(
        Expression<Func<TEntity, TColumn>> propertySelector,
        TColumn value,
        string? name = null
    )
        where TColumn : IComparable
    {
        Values.Add(new KeysetColumnValue<TEntity, TColumn>(propertySelector, value, name));
        return this;
    }
}