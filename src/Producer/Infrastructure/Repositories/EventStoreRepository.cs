using Post.Command.Infrastructure.Models;
using Post.Command.Infrastructure.MongoCollection;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository<EventModel>
{
    private readonly IMongoEventCollection<EventModel> _mongoEventCollection;

    public EventStoreRepository(IMongoEventCollection<EventModel> mongoEventCollection) =>
        _mongoEventCollection = mongoEventCollection;

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId) => await _mongoEventCollection.Find(x => x.AggregateIdentifier == aggregateId);

    public async Task<List<EventModel>> FindAllAsync() => await _mongoEventCollection.Find(_ => true);

    public async Task SaveAsync(EventModel @event) => await _mongoEventCollection.InsertOneAsync(@event);
}
