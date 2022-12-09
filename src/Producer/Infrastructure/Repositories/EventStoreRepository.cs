using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Command.Infrastructure.Configuration;
using Post.Command.Infrastructure.Models;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository<EventModel>
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;
    public EventStoreRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions)
    {
        var mongoClient = new MongoClient(mongoDbConfigurationOptions.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfigurationOptions.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(mongoDbConfigurationOptions.Value.Collection);
    }
    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId) => await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync();
    public async Task<List<EventModel>> FindAllAsync() => await _eventStoreCollection.Find(_ => true).ToListAsync();

    public async Task SaveAsync(EventModel @event) => await _eventStoreCollection.InsertOneAsync(@event);
}