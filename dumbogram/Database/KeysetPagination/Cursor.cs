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
    public KeysetPaginationDirection Direction;
    public KeysetOrder<TEntity> KeysetOrder;
    public int Take;
    public CursorType Type;


    public Cursor(KeysetOrder<TEntity> keysetOrder, KeysetPaginationDirection direction, int take)
    {
        KeysetOrder = keysetOrder;
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