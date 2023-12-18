using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination.Dto;

public class PagingDetails<TEntity>(
    Keyset<TEntity> Keyset, 
    Cursor<TEntity> Cursor, 
    PaginationDirection Direction,
    int Take
) where TEntity : BaseEntity
{
    public Keyset<TEntity> Keyset { get; private set; } = null!;
    public Cursor<TEntity> Cursor { get; private set; } = null!;
    public PaginationDirection Direction { get; private set; }
    public int Take { get; private set; }
}