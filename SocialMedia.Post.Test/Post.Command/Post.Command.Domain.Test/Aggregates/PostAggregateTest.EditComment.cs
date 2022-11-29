using Post.Common.Events;

namespace Post.Command.Domain.Test.Aggregates
{
    public partial class PostAggregateTest
    {
        [Fact]
        public void EditComment_GivenCommentId_ShouldRaiseCommentUpdatedEvent()
        {
            // Arrange
            var addedComment = _fixture.Create<string>();
            var updatedComment = _fixture.Create<string>();
            var username = _fixture.Create<string>();
            _sut.AddComment(addedComment, username);
            var addedEvent = _sut.GetUncommittedChanges().OfType<CommentAddedEvent>().First();

            // Act
            _sut.EditComment(addedEvent.CommentId, updatedComment, username);

            // Assert
            _sut.Id.Should().Be(_id);
            _sut.Active.Should().BeTrue();
            var @event = _sut.GetUncommittedChanges().OfType<CommentUpdatedEvent>().First();
            @event.Id.Should().Be(_id);
            @event.Comment.Should().Be(updatedComment);
            @event.Username.Should().Be(username);
            @event.CommentId.Should().Be(addedEvent.CommentId);
        }

        [Fact]
        public void EditComment_GivenInactiveAggregate_ShouldInvalidOperationException()
        {
            // Arrange
            _sut.Active = false;
            var comment = _fixture.Create<string>();
            var username = _fixture.Create<string>();
            var commentId = _fixture.Create<Guid>();

            // Act
            var editCommentAction = () => _sut.EditComment(commentId, comment, username);

            // Assert
            editCommentAction
               .Should()
               .Throw<InvalidOperationException>()
               .WithMessage("You cannot edit comment of an inactive post");
        }


        [Theory(DisplayName = "Edit Comment when comment is null, empty or white space")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void EditComment_GivenInvalidComment_ShouldInvalidOperationException(string comment)
        {
            // Arrange
            var username = _fixture.Create<string>();
            var commentId = _fixture.Create<Guid>();

            // Act
            var editCommentAction = () => _sut.EditComment(commentId, comment, username);

            // Assert
            editCommentAction
               .Should()
               .Throw<InvalidOperationException>()
               .WithMessage($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
        }

        [Theory(DisplayName = "Edit comment when username is null, empty or white space")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void EditComment_GivenInvalidUsername_ShouldInvalidOperationException(string username)
        {
            // Arrange
            var comment = _fixture.Create<string>();
            var commentId = _fixture.Create<Guid>();

            // Act
            var editCommentAction = () => _sut.EditComment(commentId, comment, username);

            // Assert
            editCommentAction
               .Should()
               .Throw<InvalidOperationException>()
               .WithMessage($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
        }

        [Fact]
        public void EditComment_GivenNotFoundCommentId_ShouldInvalidOperationException()
        {
            // Arrange
            var comment = _fixture.Create<string>();
            var username = _fixture.Create<string>();
            var commentId = _fixture.Create<Guid>();

            // Act
            var editCommentAction = () => _sut.EditComment(commentId, comment, username);

            // Assert
            editCommentAction
               .Should()
               .Throw<InvalidOperationException>()
               .WithMessage($"You cannot edit a none existing comment");
        }

        [Fact]
        public void EditComment_GivenAnotherUser_ShouldInvalidOperationException()
        {
            // Arrange
            var addedComment = _fixture.Create<string>();
            var updatedComment = _fixture.Create<string>();
            var username = _fixture.Create<string>();
            var anotherUsername = _fixture.Create<string>();
            _sut.AddComment(addedComment, username);
            var addedEvent = _sut.GetUncommittedChanges().OfType<CommentAddedEvent>().First();

            // Act
            var editCommentAction = () => _sut.EditComment(addedEvent.CommentId, updatedComment, anotherUsername);

            // Assert
            editCommentAction
               .Should()
               .Throw<InvalidOperationException>()
               .WithMessage($"Your are not allowed to edit a comment that was made by another user");
        }
    }
}