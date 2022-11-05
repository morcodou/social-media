// using AutoFixture.Kernel;
// using CQRS.Core.Domain;
// using CQRS.Core.Events;
// using CQRS.Core.Exceptions;
// using CQRS.Core.Producers;
// using Post.Command.Infrastructure.Stores;
// using Post.Common.Events;

// namespace Post.Command.Infrastructure.Test.Stores
// {
//     public class EventStoreTest
//     {
//         private readonly EventStore _sut;
//         private readonly IEventStoreRepository _eventStoreRepository;
//         private readonly IEventProducer _eventProducer;
//         private readonly Fixture _fixture;

//         public EventStoreTest()
//         {
//             _eventStoreRepository = Mock.Of<IEventStoreRepository>();
//             _eventProducer = Mock.Of<IEventProducer>();
//             _sut = new(_eventStoreRepository, _eventProducer);

//             _fixture = new();
//             _fixture.Customizations.Add(new TypeRelay(typeof(BaseEvent), typeof(PostLikedEvent)));
//         }

//         [Fact]
//         public void GetEventsAsync_GivenEmptyNotExistingAggregateId_ShouldThrowsAggregateNotFoundException()
//         {
//             // Arrange
//             var aggregateId = _fixture.Create<Guid>();
//             List<EventModel> aggregates = new();

//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.FindByAggregateId(aggregateId))
//                 .ReturnsAsync(aggregates);

//             // Act
//             var exception = Assert.ThrowsAsync<AggregateNotFoundException>(async () => await _sut.GetEventsAsync(aggregateId));

//             // Assert
//             exception.Should().NotBeNull();
//         }

//         [Fact]
//         public void GetEventsAsync_GivenNullNotExistingAggregateId_ShouldThrowsAggregateNotFoundException()
//         {
//             // Arrange
//             var aggregateId = _fixture.Create<Guid>();
//             List<EventModel> aggregates = null!;

//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.FindByAggregateId(aggregateId))
//                 .ReturnsAsync(aggregates);

//             // Act
//             var exception = Assert.ThrowsAsync<AggregateNotFoundException>(async () => await _sut.GetEventsAsync(aggregateId));

//             // Assert
//             exception.Should().NotBeNull();
//         }

//         [Fact]
//         public async Task GetEventsAsync_GivenAggregateId_ShouldReturnsAggregateEvents()
//         {
//             // Arrange
//             var aggregateId = _fixture.Create<Guid>();
//             var aggregates = _fixture.CreateMany<EventModel>().ToList();
//             var expected = aggregates.Select(x => x.EventData).ToList();

//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.FindByAggregateId(aggregateId))
//                 .ReturnsAsync(aggregates);

//             // Act
//             var result = await _sut.GetEventsAsync(aggregateId);

//             // Assert
//             result.Should().NotBeNull();
//             result.Should().BeEquivalentTo(expected);
//         }

//         [Fact]
//         public void SaveEventsAsync_GivenInvalidExpectedVersion_ShouldThrowsConcurrencyException()
//         {
//             // Arrange
//             var aggregateId = _fixture.Create<Guid>();
//             var aggregates = _fixture.CreateMany<EventModel>().ToList();
//             var expectedVersion = aggregates.Max(x => x.Version) + 1;
//             var events = _fixture.CreateMany<BaseEvent>();

//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.FindByAggregateId(aggregateId))
//                 .ReturnsAsync(aggregates);

//             // Act
//             var exception = Assert.ThrowsAsync<ConcurrencyException>(
//                 async () => await _sut.SaveEventsAsync(aggregateId, events, expectedVersion)
//             );

//             // Assert
//             exception.Should().NotBeNull();
//         }

//         [Theory]
//         [InlineData(true)]
//         [InlineData(false)]
//         public async Task SaveEventsAsync_GivenExpectedVersion_ShouldSaveAllEvents(bool newExpectedVersion)
//         {
//             // Arrange
//             var aggregateId = _fixture.Create<Guid>();
//             var eventStream = _fixture.CreateMany<EventModel>().ToList();
//             var expectedVersion = newExpectedVersion ? -1 : eventStream[^1].Version;
//             var events = _fixture.CreateMany<BaseEvent>();

//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.FindByAggregateId(aggregateId))
//                 .ReturnsAsync(eventStream);
//             Mock.Get(_eventStoreRepository)
//                 .Setup(x => x.SaveAsync(It.IsAny<EventModel>()))
//                 .Verifiable();
//             Mock.Get(_eventProducer)
//                 .Setup(x => x.ProduceAsync("socialmediapostevents", It.IsAny<BaseEvent>()))
//                 .Verifiable();
//             // Act

//             await _sut.SaveEventsAsync(aggregateId, events, expectedVersion);

//             // Assert
//             Mock.Get(_eventStoreRepository)
//                 .Verify(repository =>
//                         repository.SaveAsync(It.Is<EventModel>(x =>
//                                    x.AggregateIdentifier == aggregateId && x.Version > expectedVersion)),
//                         Times.Exactly(events.Count()));
//             Mock.Get(_eventProducer)
//                 .Verify(x => x.ProduceAsync("socialmediapostevents", It.IsAny<BaseEvent>()), Times.Exactly(events.Count()));
//         }
//     }
// }