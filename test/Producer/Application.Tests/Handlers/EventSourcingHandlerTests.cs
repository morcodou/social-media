using AutoFixture.Kernel;
using Post.Command.Application.Handlers;
using Post.Command.Application.Infrastructure;

namespace Post.Command.Infrastructure.Handlers;

public class EventSourcingHandlerTests
{
    public class FakePostAggregate : AggregateRoot
    {
        public FakePostAggregate()
        { }

        public override void ReplayEvents(IEnumerable<BaseEvent> events) =>
            ReplayedEvents.AddRange(events);

        public List<BaseEvent> ReplayedEvents = new List<BaseEvent>();
    }

    private readonly EventSourcingHandler<FakePostAggregate> _sut;
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;
    private readonly Fixture _fixture;
    private const string _TOPIC_NAME = "socialmediapostevents";

    public EventSourcingHandlerTests()
    {
        _eventStore = Mock.Of<IEventStore>();
        _eventProducer = Mock.Of<IEventProducer>();
        _sut = new(_eventStore, _eventProducer);

        _fixture = new();
        _fixture.Customizations.Add(new TypeRelay(typeof(BaseEvent), typeof(PostLikedEvent)));
    }

    //public async Task RepublishEventsAsync()
    //{
    //    var aggregateIds = await _eventStore.GetAggregateIdsAsync();
    //    if (aggregateIds.IsNullOrEmpty()) return;
    //    foreach (var aggregateId in aggregateIds)
    //    {
    //        var aggregate = await GetByIdAsync(aggregateId);
    //        if (aggregate == null) continue;
    //        var events = await _eventStore.GetEventsAsync(aggregateId);
    //        foreach (var @event in events)
    //        {
    //            var topic = $"{Environment.GetEnvironmentVariable("KAFKA_TOPIC")}";
    //            await _eventProducer.ProduceAsync(topic, @event);
    //        }
    //    }
    //}

    [Fact]
    public async Task RepublishEventsAsync_GivenEmptyStore_ShouldNotPublishEvents()
    {
        // Arrange
        var aggregateIds = new List<Guid>();
        Mock.Get(_eventStore)
            .Setup(x => x.GetAggregateIdsAsync())
            .ReturnsAsync(aggregateIds);

        // Act
        await _sut.RepublishEventsAsync();

        // Assert
        Mock.Get(_eventStore)
            .Verify(x => x.GetAggregateIdsAsync(), Times.Once);
        Mock.Get(_eventStore)
            .Verify(x => x.GetEventsAsync(It.IsAny<Guid>()), Times.Never);
        Mock.Get(_eventProducer)
            .Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<BaseEvent>()), Times.Never);
    }

    [Fact]
    public async Task RepublishEventsAsync_GivenAggregateIdWithoutAggregate_ShouldNotPublishEvents()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        List<BaseEvent> events = null!;
        Mock.Get(_eventStore)
            .Setup(x => x.GetEventsAsync(aggregateId))
            .ReturnsAsync(events);

        var aggregateIds = new List<Guid>() { aggregateId };
        Mock.Get(_eventStore)
            .Setup(x => x.GetAggregateIdsAsync())
            .ReturnsAsync(aggregateIds);

        // Act
        await _sut.RepublishEventsAsync();

        // Assert
        Mock.Get(_eventStore)
            .Verify(x => x.GetAggregateIdsAsync(), Times.Once);
        Mock.Get(_eventStore)
            .Verify(x => x.GetEventsAsync(aggregateId), Times.AtLeastOnce);
        Mock.Get(_eventProducer)
            .Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<BaseEvent>()), Times.Never);
    }

    [Fact]
    public async Task RepublishEventsAsync_GivenAggregateIdWithAggregate_ShouldPublishEvents()
    {
        // Arrange
        Environment.SetEnvironmentVariable("KAFKA_TOPIC", _TOPIC_NAME);
        var aggregateId = Guid.NewGuid();
        var events = _fixture.CreateMany<BaseEvent>().ToList();
        Mock.Get(_eventStore)
            .Setup(x => x.GetEventsAsync(aggregateId))
            .ReturnsAsync(events);

        var aggregateIds = new List<Guid>() { aggregateId };
        Mock.Get(_eventStore)
            .Setup(x => x.GetAggregateIdsAsync())
            .ReturnsAsync(aggregateIds);

        // Act
        await _sut.RepublishEventsAsync();

        // Assert
        Mock.Get(_eventStore)
            .Verify(x => x.GetAggregateIdsAsync(), Times.Once);
        Mock.Get(_eventStore)
            .Verify(x => x.GetEventsAsync(aggregateId), Times.AtLeastOnce);
        Mock.Get(_eventProducer)
            .Verify(x => x.ProduceAsync(_TOPIC_NAME, It.Is<BaseEvent>(ev => events.Contains(ev))), Times.Exactly(events.Count));
    }

    [Fact]
    public async Task GetByIdAsync_GivenAggregateId_ShouldReturnsAggregate()
    {
        // Arrange
        var aggregateId = _fixture.Create<Guid>();
        var events = _fixture.CreateMany<BaseEvent>().ToList();
        var version = events.Max(x => x.Version);
        Mock.Get(_eventStore)
            .Setup(x => x.GetEventsAsync(aggregateId))
            .ReturnsAsync(events);

        // Act
        var result = await _sut.GetByIdAsync(aggregateId);

        // Assert
        result.Should().NotBeNull();
        result.Version.Should().Be(version);
        result.ReplayedEvents.Should().BeEquivalentTo(events);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetByIdAsync_GivenNotFoundAggregateId_ShouldReturnsEmptyAggregate(bool isNull)
    {
        // Arrange
        var aggregateId = _fixture.Create<Guid>();
        List<BaseEvent> events = isNull ? null! : new List<BaseEvent>();
        var version = -1;
        Mock.Get(_eventStore)
            .Setup(x => x.GetEventsAsync(aggregateId))
            .ReturnsAsync(events);

        // Act
        var result = await _sut.GetByIdAsync(aggregateId);

        // Assert
        result.Should().NotBeNull();
        result.Version.Should().Be(version);
        result.ReplayedEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task SaveAsync_GivenAggregate_ShouldCommitChanges()
    {
        // Arrange
        var aggregateId = _fixture.Create<Guid>();
        var version = _fixture.Create<int>();
        var aggregate = Mock.Of<FakePostAggregate>(x => x.Id == aggregateId && x.Version == version);
        var changes = _fixture.CreateMany<BaseEvent>();
        Mock.Get(aggregate)
            .Setup(x => x.GetUncommittedChanges())
            .Returns(changes);

        // Act
        await _sut.SaveAsync(aggregate);

        // Assert
        Mock.Get(aggregate)
            .Verify(x => x.MarkChangesAsCommitted(), Times.Once);
        Mock.Get(_eventStore)
            .Verify(x => x.SaveEventsAsync(aggregate.Id, changes, aggregate.Version), Times.Once);
    }
}

//using CQRS.Core.Handlers;
//using Post.Command.Api.Commands;
//using Post.Command.Domain.Aggregates;
//using Post.Command.Domain.Factories;

//namespace Post.Command.Api.Test.Commands
//{
//    public class CommandHandlerTest
//    {
//        private readonly CommandHandler<PostAggregateBase> _sut;
//        private readonly Fixture _fixture;
//        private readonly IEventSourcingHandler<PostAggregateBase> _eventSourcingHandler;
//        private readonly IPostAggregateFactory<PostAggregateBase> _postAggregateFactory;

//        public CommandHandlerTest()
//        {
//            _eventSourcingHandler = Mock.Of<IEventSourcingHandler<PostAggregateBase>>();
//            _postAggregateFactory = Mock.Of<IPostAggregateFactory<PostAggregateBase>>();
//            _sut = new(_eventSourcingHandler, _postAggregateFactory);

//            _fixture = new Fixture();
//        }

//        [Fact]
//        public async Task HandleAsync_NewPostCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<NewPostCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(_postAggregateFactory)
//                .Setup(f => f.Create(command.Id, command.Author, command.Message))
//                .Returns(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(_postAggregateFactory)
//                .Verify(f => f.Create(command.Id, command.Author, command.Message), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_EditMessageCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<EditMessageCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.EditMessage(command.Message))
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.EditMessage(command.Message), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_LikePostCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<LikePostCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.LikePost())
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.LikePost(), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_AddCommentCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<AddCommentCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.AddComment(command.Comment, command.Username))
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.AddComment(command.Comment, command.Username), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_EditCommentCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<EditCommentCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.EditComment(command.CommentId, command.Comment, command.Username))
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.EditComment(command.CommentId, command.Comment, command.Username), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_RemoveCommentCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<RemoveCommentCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.RemoveComment(command.CommentId, command.Username))
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.RemoveComment(command.CommentId, command.Username), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }

//        [Fact]
//        public async Task HandleAsync_DeletePostCommand()
//        {
//            // Arrange
//            var command = _fixture.Create<DeletePostCommand>();
//            var aggregate = Mock.Of<PostAggregateBase>();

//            Mock.Get(aggregate)
//                .Setup(h => h.DeletePost(command.Username))
//                .Verifiable();
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.GetByIdAsync(command.Id))
//                .ReturnsAsync(aggregate);
//            Mock.Get(_eventSourcingHandler)
//                .Setup(h => h.SaveAsync(aggregate))
//                .Verifiable();

//            // Act
//            await _sut.HandleAsync(command);

//            // Assert
//            Mock.Get(aggregate)
//                .Verify(h => h.DeletePost(command.Username), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.GetByIdAsync(command.Id), Times.Once);
//            Mock.Get(_eventSourcingHandler)
//                .Verify(h => h.SaveAsync(aggregate), Times.Once);
//        }
//    }
//}