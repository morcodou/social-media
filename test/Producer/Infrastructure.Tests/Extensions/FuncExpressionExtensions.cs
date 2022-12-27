namespace System.Linq.Expressions;

internal static class FuncExpressionExtensions
{
    public static bool IsEqualTo<TSource, TValue>(this Func<TSource, TValue> x, Func<TSource, TValue> y)
    {
        var xExpression = x.ToExpression();
        var yExpression = y.ToExpression();
        return xExpression.IsEqualTo(yExpression);
    }

    public static bool IsEqualTo<TSource, TValue>(this Expression<Func<TSource, TValue>> x, Expression<Func<TSource, TValue>> y) => x.ExpressionEqual(y);

    private static bool ExpressionEqual(this Expression x, Expression y)
    {
        if (HasAtLeastOneNull(x, y)) return false;
        if (AreReferenceEquals(x, y)) return true;
        if (AreNotSameType(x, y)) return false;

        return x.NodeType switch
        {
            ExpressionType.Lambda => ExpressionEqual(((LambdaExpression)x).Body, ((LambdaExpression)y).Body),
            ExpressionType.Invoke => ExpressionEqual(((InvocationExpression)x).Expression, ((InvocationExpression)y).Expression),
            ExpressionType.MemberAccess => x is MemberExpression xExpression &&
                                           y is MemberExpression yExpression &&
                                           xExpression.Member == yExpression.Member,
            _ => throw new NotImplementedException(x.NodeType.ToString())
        };
    }

    public static Expression<Func<TSource, TValue>> ToExpression<TSource, TValue>(this Func<TSource, TValue> func) => x => func(x);

    private static bool AreReferenceEquals(Expression x, Expression y) => ReferenceEquals(x, y);

    private static bool HasAtLeastOneNull(Expression x, Expression y) => x == null || y == null;

    private static bool AreNotSameType(Expression x, Expression y) => x.NodeType != y.NodeType || x.Type != y.Type;
}