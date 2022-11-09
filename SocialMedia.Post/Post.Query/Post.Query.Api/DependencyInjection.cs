using Confluent.Kafka;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
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
        services.AddScoped<IEventHandler, Infrastructure.Handlers.EventHandler>();
        services.AddScoped<IEventConsumer, EventConsumer>();

        var dbContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureCreated();

        return services;
    }
}