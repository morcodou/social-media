using Confluent.Kafka;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
                .Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)))
                .RegisterInfrastructureServices(configuration)
                .RegisterApplicationServices();
    }
}