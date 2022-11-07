using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _builder;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> builder)
        {
            _builder = builder;
        }

        public DatabaseContext Create()
        {
            DbContextOptionsBuilder<DatabaseContext> options = new ();
            _builder (options);

            return new DatabaseContext(options.Options);
        }
    }
}