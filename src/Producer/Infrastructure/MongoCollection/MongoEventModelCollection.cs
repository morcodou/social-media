using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Command.Infrastructure.Configuration;
using Post.Command.Infrastructure.Models;

namespace Post.Command.Infrastructure.MongoCollection;

public class MongoEventModelCollection : IMongoEventCollection<EventModel>
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public MongoEventModelCollection(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions)
    {
        var mongoClient = new MongoClient(mongoDbConfigurationOptions.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfigurationOptions.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(mongoDbConfigurationOptions.Value.Collection);
    }

    public async Task<List<EventModel>> Find(Func<EventModel, bool> filter) =>
            await _eventStoreCollection.Find(x => filter(x)).ToListAsync();

    public Task InsertOneAsync(EventModel document) => _eventStoreCollection.InsertOneAsync(document);
}