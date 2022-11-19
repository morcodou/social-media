using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Post.Query.Infrastructure.Test.Fakes
{
    internal class FakeAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _queryProvider;

        public FakeAsyncQueryProvider(IQueryProvider queryProvider) => _queryProvider = queryProvider;

        public IQueryable CreateQuery(Expression expression) => new FakeAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new FakeAsyncEnumerable<TElement>(expression);

        public object? Execute(Expression expression) => _queryProvider.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _queryProvider.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            return Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new FakeAsyncEnumerable<TResult>(expression);
        }
    }
}