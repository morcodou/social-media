namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .RegisterInfrastructureServices(configuration)
            .RegisterApplicationServices();
}