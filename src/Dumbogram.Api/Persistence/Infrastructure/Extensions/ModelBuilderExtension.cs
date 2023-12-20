using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Dumbogram.Api.Persistence.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    /// <summary>
    ///     Applies expression predicate as query filter for ALL entities that implements TInterface
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="expression"></param>
    /// <typeparam name="TInterface"></typeparam>
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => typeof(TInterface).IsAssignableFrom(e.ClrType) && e.BaseType == null)
            // .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            // Modify expression to handle correct child type
            var newParam = Expression.Parameter(entity);
            var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            var lambdaExpression = Expression.Lambda(newBody, newParam);

            // Set filter
            modelBuilder.Entity(entity).HasQueryFilter(lambdaExpression);
        }
    }
}