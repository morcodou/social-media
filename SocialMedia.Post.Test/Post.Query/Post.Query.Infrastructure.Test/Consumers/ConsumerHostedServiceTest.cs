using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Post.Query.Infrastructure.Consumers;

namespace Post.Query.Infrastructure.Test.Consumers
{
    public class ConsumerHostedServiceTest
    {
        private readonly ILogger<ConsumerHostedService> _logger = Mock.Of<ILogger<ConsumerHostedService>>();
        private readonly IEventConsumer _eventConsumer = Mock.Of<IEventConsumer>();
        private const string _CONSUMER_RUNNING = "Event consumer service running";
        private const string _CONSUMER_STOPPED = "Event consumer service stopped";
        private const string _TOPIC_NAME = "socialmediapostevents";


        [Fact]
        public async Task ConsumerHostedService_GivenConsumerAndTopic_ShouldConsumeTopic()
        {
            // Arrange
            Environment.SetEnvironmentVariable("KAFKA_TOPIC", _TOPIC_NAME);
            var sut = GetConsumerHostedService(_logger, _eventConsumer);

            // Act
            await RunConsumerHostedServiceAsync(sut!);

            // Assert
            Mock.Get(_eventConsumer)
                .Verify(c => c.Consume(_TOPIC_NAME), Times.AtLeastOnce);
            VerifyLoggerInformation();
        }

        [Fact]
        public async Task ConsumerHostedService_GivenNoTopicName_ShouldNotConsume()
        {
            // Arrange
            Environment.SetEnvironmentVariable("KAFKA_TOPIC", string.Empty);
            var sut = GetConsumerHostedService(_logger, _eventConsumer);

            // Act
            await RunConsumerHostedServiceAsync(sut!);

            // Assert
            Mock.Get(_eventConsumer).Verify(c => c.Consume(_TOPIC_NAME), Times.Never);
            VerifyLoggerInformation();
        }

        [Fact]
        public async Task ConsumerHostedService_GivenNoConsumer_ShouldNotConsume()
        {
            // Arrange
            Environment.SetEnvironmentVariable("KAFKA_TOPIC", _TOPIC_NAME);
            var sut = GetConsumerHostedService(_logger);

            // Act
            await RunConsumerHostedServiceAsync(sut!);

            // Assert
            Mock.Get(_eventConsumer).Verify(c => c.Consume(_TOPIC_NAME), Times.Never);
            VerifyLoggerInformation();
        }

        private async Task RunConsumerHostedServiceAsync(ConsumerHostedService sut)
        {
            await sut.StartAsync(CancellationToken.None);
            await Task.Delay(100);
            await sut.StopAsync(CancellationToken.None);
        }

        private void VerifyLoggerInformation()
        {
            Mock.Get(_logger)
                .Verify(x => x.Log(LogLevel.Information,
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((m, _) => $"{m}".Contains(_CONSUMER_STOPPED) || $"{m}".Contains(_CONSUMER_RUNNING)),
                                It.IsAny<Exception>(),
                                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Exactly(2));
        }

        private ConsumerHostedService GetConsumerHostedService(
            ILogger<ConsumerHostedService> logger,
            IEventConsumer? eventConsumer = null)
        {
            var services = new ServiceCollection();
            if (eventConsumer != null)
                services.AddSingleton(eventConsumer);
            services.AddSingleton(logger);
            services.AddHostedService<ConsumerHostedService>();
            var serviceProvider = services.BuildServiceProvider();
            var consumerHostedService = serviceProvider.GetService<IHostedService>() as ConsumerHostedService;
            return consumerHostedService!;
        }
    }
}