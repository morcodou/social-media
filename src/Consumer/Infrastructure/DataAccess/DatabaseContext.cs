namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<PostEntity> Posts { get; set; } = null!;
    public virtual DbSet<CommentEntity> Comments { get; set; } = null!;
}