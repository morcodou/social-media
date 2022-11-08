using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _factory;

        public PostRepository(DatabaseContextFactory factory)
        {
            _factory = factory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using var dbContext = _factory.Create();
            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using var dbContext = _factory.Create();
            var postEntity = await GetByIdAsync(postId);
            if (postEntity == null) return;

            dbContext.Posts.Remove(postEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using var dbContext = _factory.Create();
            var postEntity = await dbContext.Posts
                                    .Include(x => x.Comments)
                                    .FirstOrDefaultAsync(x => x.PostId == postId);
            return postEntity;
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using var dbContext = _factory.Create();
            return await dbContext.Posts.AsNoTracking()
                 .Include(x => x.Comments).AsNoTracking()
                 .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string author)
        {
            using var dbContext = _factory.Create();
            return await dbContext.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using var dbContext = _factory.Create();
            return await dbContext.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Comments.SafeAny())
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using var dbContext = _factory.Create();
            return await dbContext.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes)
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using var dbContext = _factory.Create();
            dbContext.Posts.Update(post);
            await dbContext.SaveChangesAsync();
        }
    }
}