namespace System.Linq.Expressions;

public class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public FakeAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    {
    }

    public FakeAsyncEnumerable(Expression expression)
        : base(expression)
    {
    }

    IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
    {
        return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    public IAsyncEnumerator<T> GetEnumerator()
    {
        return this.GetAsyncEnumerator();
    }
}