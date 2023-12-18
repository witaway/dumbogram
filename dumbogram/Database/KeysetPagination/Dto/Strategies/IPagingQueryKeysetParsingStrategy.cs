using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination.Dto;

public interface IPagingQueryKeysetParsingStrategy<TEntity> where TEntity : BaseEntity
{
    public Keyset<TEntity> GetKeyset(string order);

    public Keyset<TEntity> GetDefaultKeyset();
}