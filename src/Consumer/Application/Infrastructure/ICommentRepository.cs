namespace Post.Query.Application.Infrastructure;

public interface ICommentRepository
{
    Task CreateAsync(CommentEntity comment);

    Task UpdateAsync(CommentEntity comment);

    Task DeleteAsync(Guid commentId);

    Task<CommentEntity> GetByIdAsync(Guid commentId);
}