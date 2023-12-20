using System.Linq.Expressions;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Internals;

public interface IKeysetColumnValue<TEntity> : IKeysetColumn<TEntity>
    where TEntity : BaseEntity
{
    public ConstantExpression ValueExpression { get; }
}

public class KeysetColumnValue<TEntity, TColumn> : KeysetColumn<TEntity, TColumn>, IKeysetColumnValue<TEntity>
    where TEntity : BaseEntity
    where TColumn : IComparable
{
    public readonly TColumn Value;

    public KeysetColumnValue(Expression<Func<TEntity, TColumn>> propertySelector, TColumn value, string? name = null)
        : base(propertySelector, name)
    {
        Value = value;
    }

    public ConstantExpression ValueExpression => Expression.Constant(Value);
}