using CQRS.Core.Handlers;
using Post.Command.Api.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Domain.Factories;

namespace Post.Command.Api.Test.Commands
{
    public class CommandHandlerTest
    {
        private readonly CommandHandler<PostAggregateBase> _sut;
        private readonly Fixture _fixture;
        private readonly IEventSourcingHandler<PostAggregateBase> _eventSourcingHandler;
        private readonly IPostAggregateFactory<PostAggregateBase> _postAggregateFactory;

        public CommandHandlerTest()
        {
            _eventSourcingHandler = Mock.Of<IEventSourcingHandler<PostAggregateBase>>();
            _postAggregateFactory = Mock.Of<IPostAggregateFactory<PostAggregateBase>>();
            _sut = new(_eventSourcingHandler, _postAggregateFactory);
           
            _fixture = new Fixture();
        }

        [Fact]
        public async Task HandleAsync_NewPostCommand()
        {
            // Arrange
            var command = _fixture.Create<NewPostCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(_postAggregateFactory)
                .Setup(f => f.Create(command.Id, command.Author, command.Message))
                .Returns(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(_postAggregateFactory)
                .Verify(f => f.Create(command.Id, command.Author, command.Message), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_EditMessageCommand()
        {
            // Arrange
            var command = _fixture.Create<EditMessageCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.EditMessage(command.Message))
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.EditMessage(command.Message), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_LikePostCommand()
        {
            // Arrange
            var command = _fixture.Create<LikePostCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.LikePost())
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.LikePost(), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_AddCommentCommand()
        {
            // Arrange
            var command = _fixture.Create<AddCommentCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.AddComment(command.Comment, command.Username))
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.AddComment(command.Comment, command.Username), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_EditCommentCommand()
        {
            // Arrange
            var command = _fixture.Create<EditCommentCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.EditComment(command.CommentId, command.Comment, command.Username))
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.EditComment(command.CommentId, command.Comment, command.Username), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_RemoveCommentCommand()
        {
            // Arrange
            var command = _fixture.Create<RemoveCommentCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.RemoveComment(command.CommentId, command.Username))
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.RemoveComment(command.CommentId, command.Username), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_DeletePostCommand()
        {
            // Arrange
            var command = _fixture.Create<DeletePostCommand>();
            var aggregate = Mock.Of<PostAggregateBase>();

            Mock.Get(aggregate)
                .Setup(h => h.DeletePost(command.Username))
                .Verifiable();
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.GetByIdAsync(command.Id))
                .ReturnsAsync(aggregate);
            Mock.Get(_eventSourcingHandler)
                .Setup(h => h.SaveAsync(aggregate))
                .Verifiable();

            // Act
            await _sut.HandleAsync(command);

            // Assert
            Mock.Get(aggregate)
                .Verify(h => h.DeletePost(command.Username), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
            Mock.Get(_eventSourcingHandler)
                .Verify(h => h.SaveAsync(aggregate), Times.Once);
        }
    }
}