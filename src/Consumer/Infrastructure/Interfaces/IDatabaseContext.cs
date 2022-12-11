namespace Post.Query.Infrastructure.Interfaces;

public interface IDatabaseContext : IDisposable
{
    DbSet<CommentEntity> Comments { get; set; }
    DbSet<PostEntity> Posts { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}