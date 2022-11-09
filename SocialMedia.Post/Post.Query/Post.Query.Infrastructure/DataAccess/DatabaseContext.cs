using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<PostEntity> Posts { get; set; } = null!;
        public virtual DbSet<CommentEntity> Comments { get; set; } = null!;
    }
}