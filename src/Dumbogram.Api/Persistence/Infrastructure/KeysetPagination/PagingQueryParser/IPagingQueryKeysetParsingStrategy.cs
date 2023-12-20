using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser;

public interface IPagingQueryKeysetParsingStrategy<TEntity> where TEntity : BaseEntity
{
    public Keyset<TEntity> GetKeyset(string order);

    public Keyset<TEntity> GetDefaultKeyset();
}