using Post.Query.Application.Infrastructure;
using Post.Query.Application.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Application.Handlers;

public class QueryHandlerTests
{
    private readonly QueryHandler _sut;
    private readonly IPostRepository _postRepository;
    private readonly Fixture _fixture;
    public QueryHandlerTests()
    {
        _postRepository = Mock.Of<IPostRepository>();
        _sut = new QueryHandler(_postRepository);

        _fixture = new();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task HandleAsync_GivenFindAllPostsQuery_ReturnsAllPosts()
    {
        // Arrange
        var query = _fixture.Create<FindAllPostsQuery>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        Mock.Get(_postRepository)
            .Setup(x => x.ListAllAsync())
            .ReturnsAsync(posts);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task HandleAsync_GivenFindPostByIdQuery_ReturnsPost()
    {
        // Arrange
        var query = _fixture.Create<FindPostByIdQuery>();
        var post = _fixture.Create<PostEntity>();
        Mock.Get(_postRepository)
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(post);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result
            .Should()
            .BeEquivalentTo(new List<PostEntity>() { post });
    }

    [Fact]
    public async Task HandleAsync_GivenNotFoundFindPostByIdQuery_ReturnsEmpty()
    {
        // Arrange
        var query = _fixture.Create<FindPostByIdQuery>();
        PostEntity post = null!;
        Mock.Get(_postRepository)
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(post);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_GivenFindPostsByAurhorQuery_ReturnsAurhorPosts()
    {
        // Arrange
        var query = _fixture.Create<FindPostsByAurhorQuery>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        Mock.Get(_postRepository)
            .Setup(x => x.ListByAuthorAsync(query.Author))
            .ReturnsAsync(posts);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task HandleAsync_GivenFindPostsWithCommentsQuery_ReturnsPostsWithComments()
    {
        // Arrange
        var query = _fixture.Create<FindPostsWithCommentsQuery>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        Mock.Get(_postRepository)
            .Setup(x => x.ListWithCommentsAsync())
            .ReturnsAsync(posts);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task HandleAsync_GivenFindPostsWithLikesQuery_ReturnsPostsWithLikes()
    {
        // Arrange
        var query = _fixture.Create<FindPostsWithLikesQuery>();
        var posts = _fixture.CreateMany<PostEntity>().ToList();
        Mock.Get(_postRepository)
            .Setup(x => x.ListWithLikesAsync(query.NumberOfLikes))
            .ReturnsAsync(posts);

        // Act
        var result = await _sut.HandleAsync(query);

        // Assert
        result.Should().BeEquivalentTo(posts);
    }
}