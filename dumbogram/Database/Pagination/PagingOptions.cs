using Dumbogram.Models.Base;

namespace Dumbogram.Database.Pagination;

public enum PagingMode
{
    First,
    Last,
    Cursor
}

public enum PagingOrder
{
    Before,
    After
}

public class PagingOptions<TEntity> where TEntity : BaseEntity
{
    public Func<TEntity, IComparable> PropertySelector;
    public int Count { get; set; } = 50;
    public IComparable Reference { get; set; }
    public PagingOrder PagingOrder { get; set; }
    public required PagingMode PagingMode { get; set; }

    public static PagingOptions<TEntity> Before(Func<TEntity, IComparable> propertySelector, IComparable reference)
    {
        return new PagingOptions<TEntity>
        {
            PagingMode = PagingMode.Cursor,
            PropertySelector = propertySelector,
            Reference = reference,
            PagingOrder = PagingOrder.Before
        };
    }

    public static PagingOptions<TEntity> After(Func<TEntity, IComparable> propertySelector, IComparable reference)
    {
        return new PagingOptions<TEntity>
        {
            PagingMode = PagingMode.Cursor,
            PropertySelector = propertySelector,
            Reference = reference,
            PagingOrder = PagingOrder.After
        };
    }

    public static PagingOptions<TEntity> First(Func<TEntity, IComparable> propertySelector)
    {
        return new PagingOptions<TEntity>
        {
            PagingMode = PagingMode.First,
            PropertySelector = propertySelector
        };
    }

    public static PagingOptions<TEntity> Last(Func<TEntity, IComparable> propertySelector)
    {
        return new PagingOptions<TEntity>
        {
            PagingMode = PagingMode.First,
            PropertySelector = propertySelector
        };
    }

    public PagingOptions<TEntity> Take(int count)
    {
        Count = count;
        return this;
    }
}