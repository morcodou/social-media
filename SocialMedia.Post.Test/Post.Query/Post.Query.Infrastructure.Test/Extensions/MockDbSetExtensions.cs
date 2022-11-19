using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.Test.Fakes;

namespace Moq
{
    internal static class MockDbSetExtensions
    {

        internal static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            return mockSet;
        }

        internal static void SetMockDbSet<T>(this Mock<DbSet<T>> mockSet, ICollection<T> entities) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
        }

        public static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(List<TEntity> entities)
            where TEntity : class
        {
            var mockSet = new Mock<DbSet<TEntity>>();

            IQueryable<TEntity> data = entities.AsQueryable();
            mockSet.As<IAsyncEnumerable<TEntity>>()
                   .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new FakeAsyncEnumerator<TEntity>(data.GetEnumerator()));
            mockSet.As<IQueryable<TEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new FakeAsyncQueryProvider<TEntity>(data.Provider));
            mockSet.As<IQueryable<TEntity>>()
                   .Setup(m => m.Expression)
                   .Returns(data.Expression);
            mockSet.As<IQueryable<TEntity>>()
                   .Setup(m => m.ElementType)
                   .Returns(data.ElementType);
            mockSet.As<IQueryable<TEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(data.GetEnumerator());

            return mockSet;
        }
    }
}
