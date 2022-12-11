using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Interfaces;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Action<DbContextOptionsBuilder> configureDbContext = configuration.GetDbContextOptionsBuilder();

       services
            .AddDbContext<IDatabaseContext, DatabaseContext>(configureDbContext)
            .AddSingleton<IDatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

        configuration
            .BinConfigurationOption<ConsumerConfig>();

        services
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<IPostRepository, PostRepository>();

        services
            .AddScoped<IQueryEventHandler, QueryEventHandler>()
            .AddScoped<IEventConsumer, EventConsumer>();

        services
            .BuildServiceProvider()
            .GetRequiredService<DatabaseContext>()
            .Database
            .EnsureCreated();

        return services;
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

    private static Action<DbContextOptionsBuilder> GetDbContextOptionsBuilder(this IConfiguration configuration)
    {
        Action<DbContextOptionsBuilder> configureDbContext = null!;
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment?.EndsWith("PostgreSQL") ?? false)
        {
            configureDbContext = (o => o.UseLazyLoadingProxies().UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
        }
        else
        {
            configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("SqlServer")));
        }

        return configureDbContext;
    }
}