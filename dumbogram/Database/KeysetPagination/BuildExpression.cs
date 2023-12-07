﻿using System.Linq.Expressions;
using System.Reflection;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public class CBuildExpression<TEntity> where TEntity : BaseEntity
{
    protected static Func<Expression, Expression, BinaryExpression> GetComparisonExpressionToApply<TEntity>(
        KeysetPaginationDirection direction,
        IKeysetColumnOrder<TEntity> column,
        bool orEqual
    )
        where TEntity : BaseEntity
    {
        var greaterThan = direction switch
        {
            KeysetPaginationDirection.Backward when column.Order == KeysetColumnOrder.Ascending => true,
            KeysetPaginationDirection.Backward when column.Order == KeysetColumnOrder.Descending => false,
            KeysetPaginationDirection.Forward when column.Order == KeysetColumnOrder.Ascending => false,
            KeysetPaginationDirection.Forward when column.Order == KeysetColumnOrder.Descending => true,
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

    protected static BinaryExpression MakeComparisonExpression<T>(
        IKeysetColumnOrder<TEntity> column,
        Expression currentColumnAccessExpression,
        Expression referenceValueExpression,
        Func<Expression, Expression, BinaryExpression> compare)
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

    protected static MethodInfo GetCompareToMethod(Type type)
    {
        var methodInfo = type.GetTypeInfo().GetMethod(nameof(string.CompareTo), new[] { type });
        if (methodInfo == null)
        {
            throw new InvalidOperationException($"Didn't find a CompareTo method on type {type.Name}.");
        }

        return methodInfo;
    }

    public static Expression<Func<TEntity, bool>> BuildExpression(
        KeysetOrder<TEntity> keysetOrder,
        KeysetPaginationDirection direction,
        Cursor<TEntity> cursor
    )
    {
        var param = Expression.Parameter(typeof(TEntity), "entity");

        var finalExpression = BuildExpression(keysetOrder, direction, cursor, param);

        return Expression.Lambda<Func<TEntity, bool>>(finalExpression, param);
    }

    public static Expression BuildExpression(
        KeysetOrder<TEntity> keysetOrder,
        KeysetPaginationDirection direction,
        Cursor<TEntity> cursor,
        ParameterExpression param
    )
    {
        Expression finalExpression;

        var orExpression = default(BinaryExpression);

        for (var i = 0; i < keysetOrder.Columns.Count; i++)
        {
            var andExpression = default(BinaryExpression);

            for (var j = 0; j <= i; j++)
            {
                var isInnerLastOperation = j == i;

                var currentColumn = keysetOrder.Columns[j];
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
}