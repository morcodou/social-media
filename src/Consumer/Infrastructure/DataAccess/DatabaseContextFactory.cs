namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContextFactory : IDatabaseContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _builder;

    public DatabaseContextFactory(Action<DbContextOptionsBuilder> builder)
    {
        _builder = builder;
    }

    public IDatabaseContext Create()
    {
        DbContextOptionsBuilder<DatabaseContext> options = new();
        _builder(options);

        return new DatabaseContext(options.Options);
    }
}