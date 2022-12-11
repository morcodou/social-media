using Post.Query.Application.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IQueryHandler, QueryHandler>();
        services.AddHostedService<ConsumerHostedService>();
        services.RegisterQueryHandlers();
        return services;
    }

    private static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
    {
        var queryHandler = services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
        var dispatcher = new QueryDispatcher();
        dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandleAsync);
        dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
        dispatcher.RegisterHandler<FindPostsByAurhorQuery>(queryHandler.HandleAsync);
        dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandleAsync);
        dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandleAsync);
        services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);

        return services;
    }
}