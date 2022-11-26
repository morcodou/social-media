using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Command.Api.Commands;
using Post.Command.Api.Interfaces.Commands;
using Post.Command.Infrastructure.Configs;
using Post.Command.Infrastructure.Dispatchers;
using Post.Command.Infrastructure.Handlers;
using Post.Command.Infrastructure.Repositories;
using Post.Command.Infrastructure.Stores;
using Post.Command.Domain.Aggregates;
using Post.Command.Domain.Factories;
using Confluent.Kafka;
using Post.Command.Infrastructure.Producers;
using CQRS.Core.Producers;

namespace Post.Command.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfig>(configuration.GetSection(nameof(MongoDbConfig)));
            services.Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)));

            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IKafkaProducerBuilder, KafkaProducerBuilder>();
            services.AddScoped<IEventProducer, EventProducer>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();
            services.AddScoped<IPostAggregateFactory<PostAggregate>, PostAggregateFactory>();
            services.AddScoped<ICommandHandler, CommandHandler<PostAggregate>>();

            services.RegisterCommandHandlers();

            return services;
        }

        private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();
            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RestoreReadDatabaseCommand>(commandHandler.HandleAsync);
            services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

            return services;
        }
    }
}