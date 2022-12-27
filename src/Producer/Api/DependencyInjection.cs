using Confluent.Kafka;
using Post.Command.Infrastructure.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)))
            .Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)))
            .RegisterInfrastructureServices(configuration)
            .RegisterApplicationServices();

    // configuration
    //     .BinConfigurationOption<MongoDbConfiguration>()
    //     .BinConfigurationOption<ProducerConfig>();
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