using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using Post.Command.Infrastructure.Configuration;
using Post.Command.Infrastructure.Models;
using Post.Command.Infrastructure.MongoCollection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterEventsMapping();
        // configuration
        //     .BinConfigurationOption<MongoDbConfiguration>()
        //     .BinConfigurationOption<ProducerConfig>();

        // services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
        // services.Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)));
        services
            .AddScoped<IMongoEventCollection<EventModel>, MongoEventModelCollection>()
            .AddScoped<IEventStoreRepository<EventModel>, EventStoreRepository>()
            .AddScoped<IKafkaProducerBuilder, KafkaProducerBuilder>()
            .AddScoped<IEventProducer, EventProducer>()
            .AddScoped<IEventStore, EventStore>();

        return services;
    }

    private static void RegisterEventsMapping()
    {
        BsonClassMap.RegisterClassMap<BaseEvent>();
        BsonClassMap.RegisterClassMap<PostCreatedEvent>();
        BsonClassMap.RegisterClassMap<PostRemovedEvent>();
        BsonClassMap.RegisterClassMap<PostLikedEvent>();
        BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
        BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
        BsonClassMap.RegisterClassMap<CommentAddedEvent>();
        BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
    }
}