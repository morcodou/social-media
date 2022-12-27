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
            ExpressionType.Constant => EqualConstantExpression(x, y),
            ExpressionType.Equal => EqualBinaryExpression(x, y),
            ExpressionType.Invoke => EqualInvocationExpression(x, y),
            ExpressionType.Lambda => EqualLambdaExpression(x, y),
            ExpressionType.MemberAccess => EqualMemberExpression(x, y),
            _ => throw new NotImplementedException(x.NodeType.ToString())
        };
    }

    private static bool EqualConstantExpression(Expression x, Expression y)
    {
        return x is ConstantExpression xConstantExpression &&
               y is ConstantExpression yConstantExpressionn &&
               $"{xConstantExpression.Value}" == $"{yConstantExpressionn.Value}";
    }

    private static bool EqualBinaryExpression(Expression x, Expression y)
    {
        return x is BinaryExpression xBinaryExpression &&
               y is BinaryExpression yBinaryExpression &&
               xBinaryExpression.Method == yBinaryExpression.Method &&
               ExpressionEqual(xBinaryExpression.Left, yBinaryExpression.Left); // &&
               // ExpressionEqual(xBinaryExpression., yBinaryExpression.Right);
    }

    private static bool EqualLambdaExpression(Expression x, Expression y)
    {
        return x is LambdaExpression xLambdaExpression &&
               y is LambdaExpression yLambdaExpression &&
               ExpressionEqual(xLambdaExpression.Body, yLambdaExpression.Body);
    }

    private static bool EqualInvocationExpression(Expression x, Expression y)
    {
        return x is InvocationExpression xInvocationExpression &&
               y is InvocationExpression yInvocationExpression &&
               ExpressionEqual(xInvocationExpression.Expression, yInvocationExpression.Expression);
    }

    private static bool EqualMemberExpression(Expression x, Expression y)
    {
        return x is MemberExpression xMemberExpression &&
               y is MemberExpression yMemberExpression &&
               xMemberExpression.Member == yMemberExpression.Member;
    }

    public static Expression<Func<TSource, TValue>> ToExpression<TSource, TValue>(this Func<TSource, TValue> func) => x => func(x);

    private static bool AreReferenceEquals(Expression x, Expression y) => ReferenceEquals(x, y);

    private static bool HasAtLeastOneNull(Expression x, Expression y) => x == null || y == null;

    private static bool AreNotSameType(Expression x, Expression y) => x.NodeType != y.NodeType || x.Type != y.Type;
}