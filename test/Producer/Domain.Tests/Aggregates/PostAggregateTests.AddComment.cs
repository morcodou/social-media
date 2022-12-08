namespace Post.Command.Domain.Aggregates;

public class PostAggregateAddCommentTests : PostAggregateBaseTests
{
    [Fact]
    public void AddComment_GivenCommentAndUsername_ShouldRaiseCommentAddedEvent()
    {
        // Arrange
        var comment = _fixture.Create<string>();
        var username = _fixture.Create<string>();

        // Act
        _sut.AddComment(comment, username);

        // Assert
        _sut.Id.Should().Be(_id);
        _sut.Active.Should().BeTrue();
        var @event = _sut.GetUncommittedChanges().OfType<CommentAddedEvent>().First();
        @event.Id.Should().Be(_id);
        @event.Comment.Should().Be(comment);
        @event.Username.Should().Be(username);
    }

    [Fact]
    public void AddComment_GivenInactiveAggregate_ShouldInvalidOperationException()
    {
        // Arrange
        _sut.Active = false;
        var comment = _fixture.Create<string>();
        var username = _fixture.Create<string>();

        // Act
        var addCommentAction = () => _sut.AddComment(comment, username);

        // Assert
        addCommentAction.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("You cannot add comment to an inactive post");
    }

    [Theory(DisplayName = "Add Comment when comment is null, empty or white space")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void AddComment_GivenInvalidComment_ShouldInvalidOperationException(string comment)
    {
        // Arrange
        var username = _fixture.Create<string>();

        // Act
        var addCommentAction = () => _sut.AddComment(comment, username);

        // Assert
        addCommentAction.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
    }

    [Theory(DisplayName = "Edit comment when username is null, empty or white space")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void AddComment_GivenInvalidUsername_ShouldInvalidOperationException(string username)
    {
        // Arrange
        var comment = _fixture.Create<string>();

        // Act
        var addCommentAction = () => _sut.AddComment(comment, username);

        // Assert
        addCommentAction.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
    }
}