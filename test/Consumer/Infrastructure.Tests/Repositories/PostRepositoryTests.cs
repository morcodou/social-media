namespace Post.Query.Infrastructure.Repositories;

public class PostRepositoryTests
{
    private readonly PostRepository _sut;
    private readonly IDatabaseContextFactory _databaseContextFactory;
    private readonly Fixture _fixture;

    public PostRepositoryTests()
    {
        _databaseContextFactory = Mock.Of<IDatabaseContextFactory>();
        _sut = new(_databaseContextFactory);

        _fixture = new();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task CreateAsync_GivenPost_ShouldAddAndSavePost()
    {
        // Arrange
        var post = _fixture.Create<PostEntity>();
        var postDbSet = Mock.Of<DbSet<PostEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>(x => x.Posts == postDbSet);

        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.CreateAsync(post);

        // Assert
        Mock.Get(_databaseContextFactory)
            .Verify(f => f.Create(), Times.Once);
        Mock.Get(postDbSet)
            .Verify(d => d.Add(post), Times.Once);
        Mock.Get(databaseContext)
            .Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task GetByIdAsync_GivenId_ShouldReturnsPost()
    {
        // Arrange
        var post = _fixture.Create<PostEntity>();
        var postId = post.PostId;
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        posts.Add(post);

        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.GetByIdAsync(postId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(post);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_GivenNotFoundId_ShouldReturnsNull()
    {
        // Arrange
        var postId = _fixture.Create<Guid>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.GetByIdAsync(postId);

        // Assert
        result.Should().BeNull();
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_GivenPost_ShouldUpdatePost()
    {
        // Arrange
        var post = _fixture.Create<PostEntity>();
        var postDbSet = Mock.Of<DbSet<PostEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>(x => x.Posts == postDbSet);

        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.UpdateAsync(post);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(postDbSet)
            .Verify(f => f.Update(post), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_GivenNotFoundId_ShouldNotRemovePost()
    {
        // Arrange
        var postId = _fixture.Create<Guid>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        var postcommentDbSet = new Mock<DbSet<PostEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts, postcommentDbSet);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.DeleteAsync(postId);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.AtLeastOnce);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        postcommentDbSet
            .Verify(f => f.Remove(It.IsAny<PostEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_GivenId_ShouldRemoveComment()
    {
        // Arrange
        var post = _fixture.Create<PostEntity>();
        var postId = post.PostId;
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        posts.Add(post);

        var postDbSet = new Mock<DbSet<PostEntity>>();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts, postDbSet);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        await _sut.DeleteAsync(postId);

        // Assert
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.AtLeastOnce);
        Mock.Get(databaseContext)
            .Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        postDbSet
            .Verify(f => f.Remove(post), Times.Once);
    }

    [Fact]
    public async Task ListAllAsync_Given_ShouldReturnsAllPosts()
    {
        // Arrange
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.ListAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(posts);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task ListByAuthorAsync_GivenAuthor_ShouldReturnsPostsByAuthor()
    {
        // Arrange
        var author = _fixture.Create<string>();
        var authorPosts = _fixture.Build<PostEntity>()
                                  .With(x => x.Author, author)
                                  .CreateMany()
                                  .ToList();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        posts.AddRange(authorPosts);
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.ListByAuthorAsync(author);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(authorPosts);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task ListWithCommentsAsync_Given_ShouldReturnsPostsWithComment()
    {
        // Arrange
        var postsWithComments = _fixture.CreateMany<PostEntity>().ToList();
        var posts = _fixture.Build<PostEntity>()
                            .With(x => x.Comments, new HashSet<CommentEntity>())
                            .CreateMany()
                            .ToList();
        posts.AddRange(postsWithComments);
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.ListWithCommentsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(postsWithComments);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

    [Fact]
    public async Task ListWithLikesAsync_GivenNumberOfLikes_ShouldReturnsPostsWithGreaterOrEqualNumberOfLikes()
    {
        // Arrange
        var numberOfLikes = 10;
        Random random = new Random();
        var postsWithTenOrMoreLikes =
            _fixture.Build<PostEntity>()
                    .With(x => x.Likes, random.Next(numberOfLikes, 2 * numberOfLikes))
                    .CreateMany()
                    .ToList();
        var posts =
            _fixture.Build<PostEntity>()
                    .With(x => x.Likes, random.Next(1, numberOfLikes))
                    .CreateMany()
                    .ToList();
        posts.AddRange(postsWithTenOrMoreLikes);
        var databaseContext = Mock.Of<IDatabaseContext>();
        Mock.Get(databaseContext)
            .Setup(c => c.Posts)
            .ReturnsDbSet(posts);
        Mock.Get(_databaseContextFactory)
            .Setup(f => f.Create())
            .Returns(databaseContext);

        // Act
        var result = await _sut.ListWithLikesAsync(numberOfLikes);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(postsWithTenOrMoreLikes);
        Mock.Get(_databaseContextFactory)
           .Verify(f => f.Create(), Times.Once);
    }

}