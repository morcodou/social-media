namespace Post.Command.Domain.Aggregates;

public class PostAggregateRemoveCommentTests : PostAggregateBaseTests
{
    [Fact]
    public void RemoveComment_GivenCommentId_ShouldRaiseCommentRemovedEvent()
    {
        // Arrange
        var addedComment = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        _sut.AddComment(addedComment, username);
        var addedEvent = _sut.GetUncommittedChanges().OfType<CommentAddedEvent>().First();

        // Act
        _sut.RemoveComment(addedEvent.CommentId, username);

        // Assert
        _sut.Id.Should().Be(_id);
        _sut.Active.Should().BeTrue();
        var @event = _sut.GetUncommittedChanges().OfType<CommentRemovedEvent>().First();
        @event.Id.Should().Be(_id);
        @event.CommentId.Should().Be(addedEvent.CommentId);
    }

    [Fact]
    public void RemoveComment_GivenInactiveAggregate_ShouldInvalidOperationException()
    {
        // Arrange
        _sut.Active = false;
        var username = _fixture.Create<string>();
        var commentId = _fixture.Create<Guid>();

        // Act
        var removeCommentAction = () => _sut.RemoveComment(commentId, username);

        // Assert
        removeCommentAction
           .Should()
           .Throw<InvalidOperationException>()
           .WithMessage("You cannot remove comment of an inactive post");
    }

    [Theory(DisplayName = "Remove comment when username is null, empty or white space")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void RemoveComment_GivenInvalidUsername_ShouldInvalidOperationException(string username)
    {
        // Arrange
        var comment = _fixture.Create<string>();
        var commentId = _fixture.Create<Guid>();

        // Act
        var removeCommentAction = () => _sut.RemoveComment(commentId, username);

        // Assert
        removeCommentAction
           .Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
    }

    [Fact]
    public void RemoveComment_GivenNotFoundCommentId_ShouldInvalidOperationException()
    {
        // Arrange
        var comment = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var commentId = _fixture.Create<Guid>();

        // Act
        var removeCommentAction = () => _sut.RemoveComment(commentId, username);

        // Assert
        removeCommentAction
           .Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"You cannot remove a none existing comment");
    }

    [Fact]
    public void RemoveComment_GivenAnotherUser_ShouldInvalidOperationException()
    {
        // Arrange
        var addedComment = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var anotherUsername = _fixture.Create<string>();
        _sut.AddComment(addedComment, username);
        var addedEvent = _sut.GetUncommittedChanges().OfType<CommentAddedEvent>().First();

        // Act
        var removeCommentAction = () => _sut.RemoveComment(addedEvent.CommentId, anotherUsername);

        // Assert
        removeCommentAction
           .Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"Your are not allowed to remove a comment that was made by another user");
    }
}