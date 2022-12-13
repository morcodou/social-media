using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using Post.Command.Infrastructure.Configuration;
using Post.Command.Infrastructure.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterEventsMapping();
        configuration
            .BinConfigurationOption<MongoDbConfiguration>()
            .BinConfigurationOption<ProducerConfig>();
        services
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

    private static IConfiguration BinConfigurationOption<TOption>(this IConfiguration configuration)
            where TOption : class, new()
    {
        TOption option = new();
        configuration
            .GetSection(nameof(TOption))
            .Bind(option);

        return configuration;
    }
}