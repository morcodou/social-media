namespace Post.Query.Infrastructure.Repositories;

public class CommentRepositoryTests
{
    private readonly CommentRepository _sut;
    private readonly IDatabaseContextFactory _databaseContextFactory;
    private readonly Fixture _fixture;

    public CommentRepositoryTests()
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
        var commentDbSet = Mock.Of<DbSet<CommentEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>(x => x.Comments == commentDbSet);

        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.CreateAsync(comment);

        // Assert
        Mock.Get(_databaseContextFactory)
            .Verify(f => f.Create(), Times.Once);
        Mock.Get(commentDbSet)
            .Verify(d => d.Add(comment), Times.Once);
        Mock.Get(databaseContext)
            .Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_GivenId_ShouldReturnsComment()
    {
        // Arrange
        var comment = _fixture.Create<CommentEntity>();
        var commentId = comment.CommentId;
        var comments = _fixture.CreateMany<CommentEntity>().ToList();
        comments.Add(comment);

        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Comments)
            .ReturnsDbSet(comments);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.GetByIdAsync(commentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(comment);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_GivenNotFoundId_ShouldReturnsNull()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();
        var comments = _fixture.CreateMany<CommentEntity>().ToList();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Comments)
            .ReturnsDbSet(comments);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.GetByIdAsync(commentId);

        // Assert
        result.Should().BeNull();
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_GivenComment_ShouldUpdateComment()
    {
        // Arrange
        var comment = _fixture.Create<CommentEntity>();
        var commentDbSet = Mock.Of<DbSet<CommentEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>(x => x.Comments == commentDbSet);

        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.UpdateAsync(comment);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(commentDbSet)
            .Verify(f => f.Update(comment), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_GivenNotFoundId_ShouldNotRemoveComment()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();
        var comments = _fixture.CreateMany<CommentEntity>().ToList();
        var commentDbSet = new Mock<DbSet<CommentEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Comments)
            .ReturnsDbSet(comments, commentDbSet);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.DeleteAsync(commentId);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.AtLeastOnce);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        commentDbSet
            .Verify(f => f.Remove(It.IsAny<CommentEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_GivenId_ShouldRemoveComment()
    {
        // Arrange
        var comment = _fixture.Create<CommentEntity>();
        var commentId = comment.CommentId;
        var comments = _fixture.CreateMany<CommentEntity>().ToList();
        comments.Add(comment);

        var commentDbSet = new Mock<DbSet<CommentEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Comments)
            .ReturnsDbSet(comments, commentDbSet);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.DeleteAsync(commentId);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.AtLeastOnce);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        commentDbSet
            .Verify(f => f.Remove(comment), Times.Once);
    }

}
