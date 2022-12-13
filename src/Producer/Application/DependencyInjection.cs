using Post.Command.Application.Dispatchers;
using Post.Command.Application.Factories;
using Post.Command.Application.Handlers;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services) =>
        services
            .AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>()
            .AddScoped<IPostAggregateFactory<PostAggregate>, PostAggregateFactory>()
            .AddScoped<ICommandHandler, CommandHandler<PostAggregate>>()
            .RegisterCommandHandlers();

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