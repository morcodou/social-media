namespace Post.Query.Infrastructure.Test.Fakes
{
    internal class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public FakeAsyncEnumerator(IEnumerator<T> enumerator) => _enumerator = enumerator;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }

        public T Current => _enumerator.Current;

        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_enumerator.MoveNext());
    }
}