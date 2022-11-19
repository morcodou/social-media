using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Infrastructure.Test.Repositories
{
    public class CommentRepositoryTest
    {
        private readonly CommentRepository _sut;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly Fixture _fixture;

        public CommentRepositoryTest()
        {
            _databaseContextFactory = Mock.Of<IDatabaseContextFactory>();
            _sut = new(_databaseContextFactory);

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task CreateAsync_GivenComment_ShouldAddAndSaveComment()
        {
            // Arrange
            var comment = _fixture.Create<CommentEntity>();
            var commentdbSet = Mock.Of<DbSet<CommentEntity>>();
            var databaseContext = Mock.Of<IDatabaseContext>(x => x.Comments == commentdbSet);

            Mock.Get(_databaseContextFactory)
                .Setup(f => f.Create())
                .Returns(databaseContext);

            // Act
            await _sut.CreateAsync(comment);

            // Assert
            Mock.Get(_databaseContextFactory)
                .Verify(f => f.Create(), Times.Once);
            Mock.Get(commentdbSet)
                .Verify(d => d.Add(comment), Times.Once);
            Mock.Get(databaseContext)
                .Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // [Fact]
        // public async Task GetByIdAsync_GivenExistingId_ShouldReturnsComment()
        // {
        //     // Arrange
        //     var Id = _fixture.Create<Guid>();
        //     var comment = _fixture.Build<CommentEntity>()
        //                           .With(x => x.CommentId, Id)
        //                           .Create();
        //     var comments = new List<CommentEntity>() { comment };
        //     var commentDbSet = MockDbSetExtensions.CreateMockDbSet(comments);
        //     //Mock.Get(commentdbSet).SetMockAsyncDbSet(new List<CommentEntity>() { comment });
        //     var databaseContext = Mock.Of<IDatabaseContext>(x => x.Comments == commentDbSet.Object);

        //     Mock.Get(_databaseContextFactory)
        //         .Setup(f => f.Create())
        //         .Returns(databaseContext);

        //     // Act
        //     var result = await _sut.GetByIdAsync(comment.CommentId);

        //     // Assert
        //     Mock.Get(_databaseContextFactory)
        //         .Verify(f => f.Create(), Times.Once);
        // }

        // [Fact]
        // public async Task GetByIdAsync_GivenExistingId_ShouldReturnsComment_New()
        // {
        //     var mockedDbContext = Create.MockedDbContextFor<DatabaseContext>();
        //     var comments = _fixture.CreateMany<CommentEntity>().ToList();
        //     mockedDbContext.Set<CommentEntity>().AddRange(comments);


        //     var Id = comments.FirstOrDefault().CommentId;

        //     // var comment = _fixture.Build<CommentEntity>()
        //     //           .With(x => x.CommentId, Id)
        //     //           .Create();
        //     // var comments = new List<CommentEntity>() { comment }.AsQueryable();

        //     // var mockSet = new Mock<DbSet<CommentEntity>>();
        //     // mockSet.As<IDbAsyncEnumerable<CommentEntity>>()
        //     // .Setup(m => m.GetAsyncEnumerator())
        //     //     .Returns(new TestDbAsyncEnumerator<CommentEntity>(comments.GetEnumerator()));

        //     // var provider = Mock.Of<IAsyncQueryProvider>();
        //     // mockSet.As<IQueryable<CommentEntity>>()
        //     // .Setup(m => m.Provider)
        //     //     // .Returns(provider);
        //     //     .Returns(new TestDbAsyncQueryProvider<CommentEntity>(comments.Provider));

        //     // Mock.Get(provider)
        //     //   .Setup(f => f.ExecuteAsync<CommentEntity>(It.IsAny<Expression>(), It.IsAny<CancellationToken>()))
        //     //   .Returns(comment);

        //     // mockSet.As<IQueryable<CommentEntity>>().Setup(m => m.Expression).Returns(comments.Expression);
        //     // mockSet.As<IQueryable<CommentEntity>>().Setup(m => m.ElementType).Returns(comments.ElementType);
        //     // mockSet.As<IQueryable<CommentEntity>>().Setup(m => m.GetEnumerator()).Returns(() => comments.GetEnumerator());

        //     // var mockContext = new Mock<IDatabaseContext>();
        //     // mockContext.Setup(c => c.Comments).Returns(mockSet.Object);

        //     Mock.Get(_databaseContextFactory)
        //       .Setup(f => f.Create())
        //       .Returns(mockedDbContext);

        //     // // Act
        //     // // var result0 = await mockSet.Object.FindAsync();
        //     var result = await _sut.GetByIdAsync(Id!);

        //     // Assert
        //     Mock.Get(_databaseContextFactory)
        //         .Verify(f => f.Create(), Times.Once);

        //     //var service = new BlogService(mockContext.Object);
        //     //var blogs = await service.GetAllBlogsAsync();

        //     //Assert.AreEqual(3, blogs.Count);
        //     //Assert.AreEqual("AAA", blogs[0].Name);
        //     //Assert.AreEqual("BBB", blogs[1].Name);
        //     //Assert.AreEqual("ZZZ", blogs[2].Name);
        // }
    }
}
