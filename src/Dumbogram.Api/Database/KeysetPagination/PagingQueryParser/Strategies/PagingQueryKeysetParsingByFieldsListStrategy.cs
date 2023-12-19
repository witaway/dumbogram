using System.Linq.Expressions;
using Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;
using Dumbogram.Api.Models.Base;

namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies;

public class PagingQueryKeysetParsingByFieldsListStrategy<TEntity> : IPagingQueryKeysetParsingStrategy<TEntity> 
    where TEntity : BaseEntity
{
    private readonly Dictionary<string, Expression<Func<TEntity, IComparable>>> _fields = new();
    private Keyset<TEntity>? _defaultKeyset;
    
    public PagingQueryKeysetParsingByFieldsListStrategy<TEntity> WithField(
        string name, 
        Expression<Func<TEntity, IComparable>> propertySelector
    )
    {
        if (!_fields.TryAdd(name, propertySelector))
        {
            throw new FieldNameForStrategyAlreadySpecified($"Field {name} already specified");
        }

        return this;
    }

    public PagingQueryKeysetParsingByFieldsListStrategy<TEntity> WithDefaultKeyset(
        Keyset<TEntity> keyset
    )
    {
        if (_defaultKeyset is not null)
        {
            throw new DefaultKeysetAlreadySpecified();
        }
        _defaultKeyset = keyset;

        return this;
    }
    
    public Keyset<TEntity> GetDefaultKeyset()
    {
        if (_defaultKeyset is null)
        {
            throw new DefaultKeysetWasNotSpecified();
        }

        return _defaultKeyset;
    }
    
    public Keyset<TEntity> GetKeyset(string order)
    {
        var keyset = new Keyset<TEntity>();

        var fields = order.Split(",");
        foreach (var field in fields)
        {
            var descending = field.StartsWith("-");
            var starsWithOrderSpecifier = field.StartsWith("+") || field.StartsWith("-");

            var fieldName = starsWithOrderSpecifier
                ? field.Substring(1)
                : field;

            if (!_fields.TryGetValue(fieldName, out var propertySelector))
            {
                throw new CannotGetValidKeysetForQuery($"Firld {fieldName} is not specified");
            }

            keyset = descending
                ? keyset.Descending(propertySelector, fieldName)
                : keyset.Ascending(propertySelector, fieldName);
        }

        return keyset;
    }
}