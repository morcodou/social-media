namespace System.Linq.Expressions;
public class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> innerEnumerator;
    private bool disposed = false;

    public FakeAsyncEnumerator(IEnumerator<T> enumerator)
    {
        this.innerEnumerator = enumerator;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return new ValueTask();
    }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return Task.FromResult(this.innerEnumerator.MoveNext());
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(Task.FromResult(this.innerEnumerator.MoveNext()));
    }

    public T Current => this.innerEnumerator.Current;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                this.innerEnumerator.Dispose();
            }

            this.disposed = true;
        }
    }
}