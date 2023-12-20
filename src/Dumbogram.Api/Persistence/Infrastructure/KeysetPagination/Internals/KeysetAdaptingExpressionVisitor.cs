﻿using System.Diagnostics;
using System.Linq.Expressions;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Internals.ExpressionBuilder;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Internals;

internal static class KeysetAdaptingExpressionVisitor
{
    /// <summary>
    ///     Takes a lambda and adapts it to the new given parameter, returning a new lambda that accesses the new parameter.
    /// </summary>
    public static Expression<Func<T, TColumn>> AdaptParameter<T, TColumn>(
        Expression<Func<T, TColumn>> expression,
        ParameterExpression newParameter)
    {
        Debug.Assert(expression.Parameters.Count == 1);

        var visitor = new KeysetParameterAdaptingExpressionVisitor<T, TColumn>(
            expression.Parameters[0],
            newParameter);
        var newBody = visitor.Visit(expression.Body);
        return Expression.Lambda<Func<T, TColumn>>(newBody, newParameter);
    }

    /// <summary>
    ///     Takes a lambda and adapts it to the new given type, returning a new lambda that accesses the equivalent properties
    ///     of the new type according to the defined loose typing rules.
    ///     Replaces a chain of member accesses only if it starts with the first parameter in the given expression.
    /// </summary>
    public static Expression<Func<object, TColumn>> AdaptType<T, TColumn>(
        Expression<Func<T, TColumn>> expression,
        Type newType)
    {
        Debug.Assert(expression.Parameters.Count == 1);

        var newParameter = Expression.Parameter(typeof(object), expression.Parameters[0].Name);
        var visitor = new KeysetTypeAdaptingExpressionVisitor<T, TColumn>(
            expression.Parameters[0],
            newParameter,
            newType);
        var newBody = visitor.Visit(expression.Body);
        return Expression.Lambda<Func<object, TColumn>>(newBody, newParameter);
    }
}

internal class KeysetParameterAdaptingExpressionVisitor<T, TColumn> : ExpressionVisitor
{
    protected readonly ParameterExpression _newParameter;
    protected readonly ParameterExpression _oldParameter;

    public KeysetParameterAdaptingExpressionVisitor(
        ParameterExpression oldParameter,
        ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newParameter : node;
    }
}

internal class KeysetTypeAdaptingExpressionVisitor<T, TColumn> : KeysetParameterAdaptingExpressionVisitor<T, TColumn>
{
    private readonly Type? _newType;

    public KeysetTypeAdaptingExpressionVisitor(
        ParameterExpression oldParameter,
        ParameterExpression newParameter,
        Type? newType)
        : base(oldParameter, newParameter)
    {
        _newType = newType;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (_newType == null)
            // Nothing to replace or adapt.
            return base.VisitMember(node);

        var startingExpression = ExpressionHelper.GetStartingExpression(node);
        if (startingExpression != _oldParameter)
            // Nothing to replace or adapt.
            return base.VisitMember(node);

        // Replace the chain of properties with the equivalent in the new type.
        var currentReplacementExpression = (Expression)Expression.Convert(Visit(startingExpression), _newType);

        var properties = ExpressionHelper.GetPropertyChain(node);

        foreach (var property in properties)
        {
            var accessor = Accessor.Obtain(currentReplacementExpression.Type);
            if (!accessor.TryGetProperty(property.Name, out var newProperty))
                throw CreateIncompatibleObjectException(property.Name);

            currentReplacementExpression = Expression.MakeMemberAccess(currentReplacementExpression, newProperty);
        }

        return currentReplacementExpression;
    }

    private Exception CreateIncompatibleObjectException(string propertyName)
    {
        var message = $"A matching property '{propertyName}' was not found on this object.";
        return new Exception(message);
    }
}