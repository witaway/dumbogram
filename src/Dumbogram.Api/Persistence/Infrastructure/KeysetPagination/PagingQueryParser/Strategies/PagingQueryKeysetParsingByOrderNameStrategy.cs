using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies;

public class PagingQueryKeysetParsingByOrderNameStrategy<TEntity> : IPagingQueryKeysetParsingStrategy<TEntity>
    where TEntity : BaseEntity
{
    private readonly Dictionary<string, Keyset<TEntity>> _keysets = new();
    private Keyset<TEntity>? _defaultKeyset;

    public Keyset<TEntity> GetDefaultKeyset()
    {
        if (_defaultKeyset is null) throw new DefaultKeysetWasNotSpecified();
        return _defaultKeyset;
    }

    public Keyset<TEntity> GetKeyset(string order)
    {
        if (!_keysets.TryGetValue(order, out var keyset))
            throw new CannotGetValidKeysetForQuery($"Name {order} was not specified");

        return keyset;
    }

    public PagingQueryKeysetParsingByOrderNameStrategy<TEntity> WithName(
        string name,
        Keyset<TEntity> keyset,
        bool isDefault = false
    )
    {
        if (!_keysets.TryAdd(name, keyset))
            throw new KeysetNameForStrategyAlreadySpecified($"Name {name} already specified");

        if (isDefault) WithDefaultKeyset(keyset);

        return this;
    }

    public PagingQueryKeysetParsingByOrderNameStrategy<TEntity> WithDefaultKeyset(
        Keyset<TEntity> keyset
    )
    {
        if (_defaultKeyset is not null) throw new DefaultKeysetAlreadySpecified();
        _defaultKeyset = keyset;
        return this;
    }
}