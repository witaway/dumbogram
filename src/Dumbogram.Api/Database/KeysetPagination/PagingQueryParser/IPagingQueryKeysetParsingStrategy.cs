using Dumbogram.Api.Models.Base;

namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser;

public interface IPagingQueryKeysetParsingStrategy<TEntity> where TEntity : BaseEntity
{
    public Keyset<TEntity> GetKeyset(string order);

    public Keyset<TEntity> GetDefaultKeyset();
}