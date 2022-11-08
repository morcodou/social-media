using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _factory;

        public CommentRepository(DatabaseContextFactory factory)
        {
            _factory = factory;
        }

        public async Task CreateAsync(CommentEntity comment)
        {
            using var dbContext = _factory.Create();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using var dbContext = _factory.Create();
            var commentEntity = await GetByIdAsync(commentId);
            if (commentEntity == null) return;

            dbContext.Comments.Remove(commentEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using var dbContext = _factory.Create();
            var commentEntity = await dbContext.Comments
                                    .FirstOrDefaultAsync(x => x.CommentId == commentId);
            return commentEntity;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using var dbContext = _factory.Create();
            dbContext.Comments.Update(comment);
            await dbContext.SaveChangesAsync();
        }
    }
}