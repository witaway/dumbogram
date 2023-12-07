using System.Linq.Expressions;
using System.Reflection;

namespace Dumbogram.Database.Pagination;

public static class PropertyPath<TSource>
{
    private static IReadOnlyList<MemberInfo> GetRaw<TResult>(Expression<Func<TSource, TResult>> expression)
    {
        var visitor = new PropertyVisitor();
        visitor.Visit(expression.Body);
        visitor.Path.Reverse();
        return visitor.Path;
    }

    public static string Get<TResult>(Expression<Func<TSource, TResult>> expression)
    {
        var path = GetRaw(expression);
        var pathNames = path.Select(p => p.Name);
        return string.Join(".", pathNames);
    }

    private class PropertyVisitor : ExpressionVisitor
    {
        internal readonly List<MemberInfo> Path = new();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Member is PropertyInfo))
            {
                throw new ArgumentException("The path can only contain properties", nameof(node));
            }

            Path.Add(node.Member);
            return base.VisitMember(node);
        }
    }
}