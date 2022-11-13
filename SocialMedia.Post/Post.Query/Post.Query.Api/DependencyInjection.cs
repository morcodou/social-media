using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Api;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Action<DbContextOptionsBuilder> configureDbContext = (o =>
        o.UseLazyLoadingProxies()
        .UseSqlServer(configuration.GetConnectionString("SqlServer")));
        services.AddDbContext<DatabaseContext>(configureDbContext);
        services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

        services.Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)));
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IQueryHandler, QueryHandler>();

        services.AddScoped<IEventHandler, Infrastructure.Handlers.EventHandler>();
        services.AddScoped<IEventConsumer, EventConsumer>();
        services.AddHostedService<ConsumerHostedService>();

        var dbContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureCreated();

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
        dispatcher.RegisterHandler<FindPostWithLikes>(queryHandler.HandleAsync);
        services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);

        return services;
    }
}