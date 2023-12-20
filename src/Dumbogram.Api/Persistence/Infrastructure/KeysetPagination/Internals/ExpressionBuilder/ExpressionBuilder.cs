using System.Linq.Expressions;
using System.Reflection;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Enum;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Internals.ExpressionBuilder;

public static class ExpressionBuilder<TEntity> where TEntity : BaseEntity
{
    public static Expression<Func<TEntity, bool>> BuildFilterExpression(
        Keyset<TEntity> keyset,
        PaginationDirection direction,
        Cursor<TEntity> cursor
    )
    {
        var param = Expression.Parameter(typeof(TEntity), "entity");

        var finalExpression = BuildFilterExpression(keyset, direction, cursor, param);

        return Expression.Lambda<Func<TEntity, bool>>(finalExpression, param);
    }

    private static Expression BuildFilterExpression(
        Keyset<TEntity> keyset,
        PaginationDirection direction,
        Cursor<TEntity> cursor,
        ParameterExpression param
    )
    {
        Expression finalExpression;

        var orExpression = default(BinaryExpression);

        for (var i = 0; i < keyset.Columns.Count; i++)
        {
            var andExpression = default(BinaryExpression);

            for (var j = 0; j <= i; j++)
            {
                var isInnerLastOperation = j == i;

                var currentColumn = keyset.Columns[j];
                var currentColumnAccessExpression = currentColumn.MakeAccessExpression(param);
                var referenceValueExpression = cursor.Values[j].ValueExpression;

                BinaryExpression innerExpression;

                if (!isInnerLastOperation)
                {
                    innerExpression = Expression.Equal(currentColumnAccessExpression, referenceValueExpression);
                }
                else
                {
                    var compare = GetComparisonExpressionToApply(direction, currentColumn, false);
                    innerExpression = MakeComparisonExpression<TEntity>(
                        currentColumn,
                        currentColumnAccessExpression,
                        referenceValueExpression,
                        compare
                    );
                }

                andExpression = andExpression == null
                    ? innerExpression
                    : Expression.And(andExpression, innerExpression);
            }

            orExpression = orExpression == null
                ? andExpression
                : Expression.Or(orExpression, andExpression!);
        }

        finalExpression = orExpression!;

        return finalExpression;
    }

    private static Func<Expression, Expression, BinaryExpression> GetComparisonExpressionToApply(
        PaginationDirection direction,
        IKeysetColumnOrder<TEntity> column,
        bool orEqual
    )
    {
        var greaterThan = direction switch
        {
            PaginationDirection.Backward when column.Order == KeysetColumnOrder.Ascending => true,
            PaginationDirection.Backward when column.Order == KeysetColumnOrder.Descending => false,
            PaginationDirection.Forward when column.Order == KeysetColumnOrder.Ascending => false,
            PaginationDirection.Forward when column.Order == KeysetColumnOrder.Descending => true,
            _ => throw new NotImplementedException()
        };

        return orEqual
            ? greaterThan
                ? Expression.GreaterThanOrEqual
                : Expression.LessThanOrEqual
            : greaterThan
                ? Expression.GreaterThan
                : Expression.LessThan;
    }

    private static BinaryExpression MakeComparisonExpression<T>(
        IKeysetColumnOrder<TEntity> column,
        Expression currentColumnAccessExpression,
        Expression referenceValueExpression,
        Func<Expression, Expression, BinaryExpression> compare
    )
    {
        var compareToMethod = GetCompareToMethod(column.Type);

        // entity.Property.CompareTo(referenceValue)
        var methodCallExpression = Expression.Call(
            currentColumnAccessExpression,
            compareToMethod,
            referenceValueExpression
        );

        // >|< 0
        return compare(methodCallExpression, Expression.Constant(0));
    }

    private static MethodInfo GetCompareToMethod(Type type)
    {
        var methodInfo = type.GetTypeInfo().GetMethod(nameof(string.CompareTo), new[] { type });
        if (methodInfo == null)
            throw new InvalidOperationException($"Didn't find a CompareTo method on type {type.Name}.");

        return methodInfo;
    }
}