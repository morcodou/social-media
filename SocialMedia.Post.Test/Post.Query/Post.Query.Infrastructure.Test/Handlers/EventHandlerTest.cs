
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Test.Handlers
{
    public class EventHandlerTest
    {
        private readonly Infrastructure.Handlers.EventHandler _sut;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly Fixture _fixture;

        public EventHandlerTest()
        {
            _postRepository = Mock.Of<IPostRepository>();
            _commentRepository = Mock.Of<ICommentRepository>();
            _sut = new(_postRepository, _commentRepository);

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task On_GivenPostCreatedEvent_ShouldCreatePost()
        {
            // Arrange
            var @event = _fixture.Create<PostCreatedEvent>();
            var predicate = (PostEntity p) => p.PostId == @event.Id
                                            && p.Author == @event.Author
                                            && p.DatePosted == @event.DatePosted
                                            && p.Message == @event.Message;

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.CreateAsync(It.Is<PostEntity>(x => predicate(x))), Times.Once);
        }

        [Fact]
        public async Task On_GivenPostLikedEvent_ShouldUpdatePostLikes()
        {
            // Arrange
            var @event = _fixture.Create<PostLikedEvent>();
            var post = _fixture.Create<PostEntity>();
            var likes = post.Likes;
            var predicate = (PostEntity p) => p.Likes == likes + 1;
            Mock.Get(_postRepository)
                 .Setup(x => x.GetByIdAsync(@event.Id))
                 .ReturnsAsync(post);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.UpdateAsync(It.Is<PostEntity>(x => predicate(x))), Times.Once);
        }

        [Fact]
        public async Task On_GivenNotFoundPostLikedEvent_ShouldNotUpdatePostLikes()
        {
            // Arrange
            var @event = _fixture.Create<PostLikedEvent>();
            PostEntity post = null!;

            Mock.Get(_postRepository)
                 .Setup(x => x.GetByIdAsync(@event.Id))
                 .ReturnsAsync(post);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.UpdateAsync(It.IsAny<PostEntity>()), Times.Never);
        }

        [Fact]
        public async Task On_GivenPostRemovedEvent_ShouldRemovePost()
        {
            // Arrange
            var @event = _fixture.Create<PostRemovedEvent>();

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.DeleteAsync(@event.Id), Times.Once);
        }

        [Fact]
        public async Task On_GivenCommentAddedEvent_ShouldCreateComment()
        {
            // Arrange
            var @event = _fixture.Create<CommentAddedEvent>();
            var predicate = (CommentEntity c) => c.Edited == false
                                            && c.PostId == @event.Id
                                            && c.CommentId == @event.CommentId
                                            && c.CommentDate == @event.CommentDate
                                            && c.Comment == @event.Comment
                                            && c.Username == @event.Username;

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_commentRepository)
                .Verify(x => x.CreateAsync(It.Is<CommentEntity>(x => predicate(x))), Times.Once);
        }

        [Fact]
        public async Task On_GivenCommentUpdatedEvent_ShouldUpdateComment()
        {
            // Arrange
            var @event = _fixture.Create<CommentUpdatedEvent>();
            var comment = _fixture.Create<CommentEntity>();
            var predicate = (CommentEntity c) => c.Edited
                                                && c.Comment == @event.Comment
                                                && c.CommentDate == @event.EditDate;

            Mock.Get(_commentRepository)
                 .Setup(x => x.GetByIdAsync(@event.CommentId))
                 .ReturnsAsync(comment);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_commentRepository)
                .Verify(x => x.UpdateAsync(It.Is<CommentEntity>(x => predicate(x))), Times.Once);
        }

        [Fact]
        public async Task On_GivenNotFoundCommentUpdatedEvent_ShouldNotUpdateComment()
        {
            // Arrange
            var @event = _fixture.Create<CommentUpdatedEvent>();
            CommentEntity comment = null!;

            Mock.Get(_commentRepository)
                 .Setup(x => x.GetByIdAsync(@event.Id))
                 .ReturnsAsync(comment);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_commentRepository)
                .Verify(x => x.UpdateAsync(It.IsAny<CommentEntity>()), Times.Never);
        }

        [Fact]
        public async Task On_GivenCommentRemovedEvent_ShouldRemoveComment()
        {
            // Arrange
            var @event = _fixture.Create<CommentRemovedEvent>();

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_commentRepository)
                .Verify(x => x.DeleteAsync(@event.CommentId), Times.Once);
        }

        [Fact]
        public async Task On_GivenMessageUpdatedEvent_ShouldUpdatePost()
        {
            // Arrange
            var @event = _fixture.Create<MessageUpdatedEvent>();
            var post = _fixture.Create<PostEntity>();
            var predicate = (PostEntity c) => c.Message == @event.Message;
            Mock.Get(_postRepository)
                 .Setup(x => x.GetByIdAsync(@event.Id))
                 .ReturnsAsync(post);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.UpdateAsync(It.Is<PostEntity>(x => predicate(x))), Times.Once);
        }

        [Fact]
        public async Task On_GivenNotFoundMessageUpdatedEvent_ShouldNotUpdatePost()
        {
            // Arrange
            var @event = _fixture.Create<CommentUpdatedEvent>();
            PostEntity post = null!;

            Mock.Get(_postRepository)
                 .Setup(x => x.GetByIdAsync(@event.Id))
                 .ReturnsAsync(post);

            // Act
            await _sut.On(@event);

            // Assert
            Mock.Get(_postRepository)
                .Verify(x => x.UpdateAsync(It.IsAny<PostEntity>()), Times.Never);
        }
    }
}