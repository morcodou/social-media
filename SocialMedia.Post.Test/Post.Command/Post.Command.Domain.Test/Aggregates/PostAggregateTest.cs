using Post.Command.Domain.Aggregates;
using Post.Common.Events;

namespace Post.Command.Domain.Test.Aggregates
{
    public partial class PostAggregateTest
    {
        private readonly PostAggregate _sut;

        private readonly Guid _id;
        private readonly string _author;
        private readonly string _message;
        private readonly Fixture _fixture;

        public PostAggregateTest()
        {
            _fixture = new();
            _id = _fixture.Create<Guid>();
            _author = _fixture.Create<string>();
            _message = _fixture.Create<string>();

            _sut = new(_id, _author, _message);
        }

        [Fact]
        public void Creation_GivenIdAndAuthorAndMessage_ShouldRaiseEventPostCreatedEvent()
        {
            // Arrange
            // Act
            // Assert
            _sut.Id.Should().Be(_id);
            _sut.Active.Should().BeTrue();
            var @event = _sut.GetUncommittedChanges().OfType<PostCreatedEvent>().First();
            @event.Id.Should().Be(_id);
            @event.Author.Should().Be(_author);
            @event.Message.Should().Be(_message);
        }

        [Fact]
        public void EditMessage_GivenMessage_ShouldRaiseMessageUpdatedEvent()
        {
            // Arrange
            var message = _fixture.Create<string>();

            // Act
            _sut.EditMessage(message);

            // Assert
            _sut.Id.Should().Be(_id);
            _sut.Active.Should().BeTrue();
            var @event = _sut.GetUncommittedChanges().OfType<MessageUpdatedEvent>().First();
            @event.Id.Should().Be(_id);
            @event.Message.Should().Be(message);
        }

        [Fact]
        public void EditMessage_GivenInactiveAggregate_ShouldInvalidOperationException()
        {
            // Arrange
            _sut.Active = false;
            var message = _fixture.Create<string>();

            // Act
            var editMessageAction = () => _sut.EditMessage(message);

            // Assert
            editMessageAction.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("You cannot edit the message of an inactive post");
        }

        [Theory(DisplayName = "Edit message when message is null, empty or white space")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void EditMessage_GivenInvalidMessage_ShouldInvalidOperationException(string message)
        {
            // Arrange
            // Act
            var editMessageAction = () => _sut.EditMessage(message);

            // Assert
            editMessageAction.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}");
        }

        [Fact]
        public void LikePost_ShouldRaisePostLikedEvent()
        {
            // Arrange
            // Act
            _sut.LikePost();

            // Assert
            _sut.Id.Should().Be(_id);
            _sut.Active.Should().BeTrue();
            var @event = _sut.GetUncommittedChanges().OfType<PostLikedEvent>().First();
            @event.Id.Should().Be(_id);
        }

        [Fact]
        public void LikePost_GivenInactiveAggregate_ShouldInvalidOperationException()
        {
            // Arrange
            _sut.Active = false;

            // Act
            var likePostAction = () => _sut.LikePost();

            // Assert
            likePostAction.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("You cannot like an inactive post");
        }

        [Fact]
        public void DeletePost_GivenAuthor_ShouldRaisePostRemovedEvent()
        {
            // Arrange
            // Act
            _sut.DeletePost(_author);

            // Assert
            _sut.Id.Should().Be(_id);
            _sut.Active.Should().BeFalse();
            var @event = _sut.GetUncommittedChanges().OfType<PostRemovedEvent>().First();
            @event.Id.Should().Be(_id);
        }

        [Fact]
        public void DeletePost_GivenInactiveAggregate_ShouldInvalidOperationException()
        {
            // Arrange
            _sut.Active = false;

            // Act
            var deletePostAction = () => _sut.DeletePost(_author);

            // Assert
            deletePostAction.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("The post has already been removed!");
        }

        [Fact]
        public void DeletePost_GivenAnotherAuthor_ShouldInvalidOperationException()
        {
            // Arrange
            var anotherAuthor = _fixture.Create<string>();

            // Act
            var deletePostAction = () => _sut.DeletePost(anotherAuthor);

            // Assert
            deletePostAction.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Your are not allowed to delete a post that was made by another user");
        }
    }
}