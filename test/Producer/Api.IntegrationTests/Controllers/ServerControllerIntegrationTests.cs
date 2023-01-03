using Microsoft.Extensions.DependencyInjection;
using Post.Command.Api.Factories;
using Post.Command.Api.Interfaces;
using Post.Command.Application.Infrastructure;
using Post.Command.Infrastructure.Models;
using Post.Command.Infrastructure.MongoCollection;

namespace Post.Command.Api.Controllers;

public class ServerSwapService : ISwapService
{
    public static string ApiVersion = "10.11.12";
    public static DateTime ServerDate = new(2020, 12, 31);

    private static readonly IServerService _serverService =
        Mock.Of<IServerService>(x => x.Version == ApiVersion && x.Now == ServerDate);

    public IServiceCollection Swap(IServiceCollection services)
    {
        return services
                .SwapScoped<IServerService>(provider => _serverService)
                .SwapScoped<IMongoEventCollection<EventModel>>(provider => MoqExtensions.GetMongoEventModelCollection())
                .SwapScoped<IEventProducer>(provider => MoqExtensions.GetEventProducer());
    }
}

public class ServerControllerIntegrationTests : IClassFixture<ProducerWebApiFactory<Program, ServerSwapService>>
{
    private readonly HttpClient _client;

    public ServerControllerIntegrationTests(ProducerWebApiFactory<Program, ServerSwapService> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDate_ShouldReturnsServerDate()
    {
        // Act
        var response = await _client.GetAsync("/api/server/date");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseDate = await response.Content.ReadAsStringAsync();
        var serverDate = DateTime.Parse(responseDate.Trim('"'));
        serverDate.Should().Be(ServerSwapService.ServerDate);
    }

    [Fact]
    public async Task GetVersion_ShouldReturnsApiVersion()
    {
        // Act
        var response = await _client.GetAsync("/api/server/version");

        // Assert
        response.EnsureSuccessStatusCode();
        var apiVersion = await response.Content.ReadAsStringAsync();
        apiVersion.Should().Be(ServerSwapService.ApiVersion);
    }
}