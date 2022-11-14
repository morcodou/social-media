using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly ILogger<ConsumerHostedService> _looger;
        private readonly IServiceProvider _serviceProvider;

        public ConsumerHostedService(
            ILogger<ConsumerHostedService> looger,
            IServiceProvider serviceProvider)
        {
            _looger = looger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _looger.LogInformation("Event consumer service running");

            using var scope = _serviceProvider.CreateScope();
            var eventConsumer = scope.ServiceProvider.GetService<IEventConsumer>();
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            if (eventConsumer != null && !string.IsNullOrEmpty(topic))
                Task.Run(() => eventConsumer.Consume(topic), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _looger.LogInformation("Event consumer service stooped");

            return Task.CompletedTask;
        }
    }
}