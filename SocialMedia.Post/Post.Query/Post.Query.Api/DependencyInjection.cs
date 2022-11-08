using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.DataAccess;

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


        var dbContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureCreated();

        return services;
    }

}
