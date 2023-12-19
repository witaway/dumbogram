using System.Linq.Expressions;
using Dumbogram.Api.Database.KeysetPagination.Internals.Utils;
using Dumbogram.Api.Models.Base;

namespace Dumbogram.Api.Database.KeysetPagination.Internals;

public interface IKeysetColumn<TEntity>
    where TEntity : BaseEntity
{
    public LambdaExpression PropertySelectorExpression { get; }
    public Type Type { get; }
    public string Path { get; set; }
    public string Name { get; set; }
    public Expression MakeAccessExpression(ParameterExpression parameter);
}

public abstract class KeysetColumn<TEntity> : IKeysetColumn<TEntity>
    where TEntity : BaseEntity
{
    public KeysetColumn(LambdaExpression propertySelectorExpression)
    {
        PropertySelectorExpression = propertySelectorExpression;
    }

    public LambdaExpression PropertySelectorExpression { get; }
    public Type Type => PropertySelectorExpression.ReturnType;
    public string Name { get; set; }
    public string Path { get; set; }

    public abstract Expression MakeAccessExpression(ParameterExpression parameter);
}

public class KeysetColumn<TEntity, TColumn> : KeysetColumn<TEntity>
    where TEntity : BaseEntity
    where TColumn : IComparable
{
    public KeysetColumn(Expression<Func<TEntity, TColumn>> propertySelectorExpression, string? name = null)
        : base(propertySelectorExpression)
    {
        Path = PropertyPath<TEntity>.Get(propertySelectorExpression);
        Name = name ?? Path;
    }

    public new Expression<Func<TEntity, TColumn>> PropertySelectorExpression =>
        (Expression<Func<TEntity, TColumn>>)base.PropertySelectorExpression;

    public Func<TEntity, TColumn> PropertySelector => PropertySelectorExpression.Compile();

    public override Expression MakeAccessExpression(ParameterExpression parameter)
    {
        return MakeAccessLambdaExpression(parameter).Body;
    }

    /// <summary>
    ///     Makes an access lambda expression for this column.
    ///     Uses the given parameter if it is not null, otherwise creates and uses a new parameter.
    /// </summary>
    private Expression<Func<TEntity, TColumn>> MakeAccessLambdaExpression(ParameterExpression? parameter = null)
    {
        parameter ??= Expression.Parameter(typeof(TEntity), "x");
        return KeysetAdaptingExpressionVisitor.AdaptParameter(PropertySelectorExpression, parameter);
    }
}