using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Enum;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination;

public class PagingDetails<TEntity>(
    Keyset<TEntity> Keyset,
    Cursor<TEntity> Cursor,
    PaginationDirection Direction,
    int Take
) where TEntity : BaseEntity
{
    public Keyset<TEntity> Keyset { get; private set; } = null!;
    public Cursor<TEntity> Cursor { get; private set; } = null!;
    public PaginationDirection Direction { get; }
    public int Take { get; }
}