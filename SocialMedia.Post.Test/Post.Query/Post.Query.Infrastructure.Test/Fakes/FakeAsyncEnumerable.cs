using System.Linq.Expressions;

namespace Post.Query.Infrastructure.Test.Fakes
{
    internal class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(Expression expression) : base(expression)
        {
        }

        public FakeAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                                => new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

    }
}