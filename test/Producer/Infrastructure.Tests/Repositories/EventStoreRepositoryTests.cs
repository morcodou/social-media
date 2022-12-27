using AutoFixture.Kernel;
using Post.Command.Infrastructure.Models;
using Post.Command.Infrastructure.MongoCollection;
using System.Linq.Expressions;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRepositoryTests
{
    private readonly EventStoreRepository _sut;
    private readonly IMongoEventCollection<EventModel> _mongoEventCollection;
    private readonly Fixture _fixture;

    public EventStoreRepositoryTests()
    {
        _mongoEventCollection = Mock.Of<IMongoEventCollection<EventModel>>();
        _sut = new EventStoreRepository(_mongoEventCollection);

        _fixture = new();
        _fixture.Customizations.Add(new TypeRelay(typeof(BaseEvent), typeof(PostLikedEvent)));
    }

    [Fact]
    public async Task FindByAggregateId_ShouldReturnsAllEventModel()
    {
        // Arrange
        var aggregateId = _fixture.Create<Guid>();
        var eventModels = _fixture.CreateMany<EventModel>().ToList();
        Func<EventModel, bool> expectedFunc = x => x.AggregateIdentifier == aggregateId;
        Mock.Get(_mongoEventCollection)
            .Setup(x => x.Find(It.Is<Func<EventModel, bool>>(func => func.IsEqualTo(expectedFunc))))
            .ReturnsAsync(eventModels);

        // Act
        var result = await _sut.FindByAggregateId(aggregateId);

        // Assert
        result.Should().BeEquivalentTo(eventModels);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnsAllEventModel()
    {
        // Arrange
        var allEventModels = _fixture.CreateMany<EventModel>().ToList();
        Func<EventModel, bool> expectedFunc = _ => true;
        Mock.Get(_mongoEventCollection)
            .Setup(x => x.Find(It.Is<Func<EventModel, bool>>(func => func.IsEqualTo(expectedFunc))))
            .ReturnsAsync(allEventModels);

        // Act
        var result = await _sut.FindAllAsync();

        // Assert
        result.Should().BeEquivalentTo(allEventModels);
    }

    [Fact]
    public async Task SaveAsync_GivenEventModel_ShouldCallInsertOneAsync()
    {
        // Arrange
        var eventModel = _fixture.Create<EventModel>();

        // Act
        await _sut.SaveAsync(eventModel);

        // Assert
        Mock.Get(_mongoEventCollection)
            .Verify(x => x.InsertOneAsync(eventModel), Times.Once);
    }
}