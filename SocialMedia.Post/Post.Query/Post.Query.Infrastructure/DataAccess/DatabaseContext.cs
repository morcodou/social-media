using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<CommentEntity> Comments { get; set; }
        DbSet<PostEntity> Posts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<PostEntity> Posts { get; set; } = null!;
        public virtual DbSet<CommentEntity> Comments { get; set; } = null!;
    }
}