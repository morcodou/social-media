using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Post.Command.Api.Factories
{
    public class ProducerWebApiFactory<TEntryPoint, TISwapService> : WebApplicationFactory<Program>
        where TEntryPoint : Program
        where TISwapService : ISwapService, new()
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                TISwapService swapService = new();
                swapService.Swap(services);
            });
        }
    }

    public interface ISwapService
    {
        IServiceCollection Swap(IServiceCollection services);
    }
}