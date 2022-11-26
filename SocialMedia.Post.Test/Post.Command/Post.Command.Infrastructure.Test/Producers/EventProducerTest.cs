using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Post.Command.Infrastructure.Producers;
using Post.Common.Events;

namespace Post.Command.Infrastructure.Test.Producers
{
    public class EventProducerTest
    {
        private readonly EventProducer _sut;
        private readonly IOptions<ProducerConfig> _producerConfigOptions;
        private readonly IKafkaProducerBuilder _producerBuilder;
        private readonly Fixture _fixture;

        private const string _TOPIC_NAME = "mocktopic";

        public EventProducerTest()
        {
            _producerBuilder = Mock.Of<IKafkaProducerBuilder>();
            _producerConfigOptions = Mock.Of<IOptions<ProducerConfig>>();
            _sut = new(_producerBuilder, _producerConfigOptions);

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [InlineData(PersistenceStatus.Persisted)]
        [InlineData(PersistenceStatus.PossiblyPersisted)]
        public async Task ProduceAsync_GivenEvent_ShouldReturnsPersisted(PersistenceStatus status)
        {
            // Arrange
            var producer = Mock.Of<IProducer<string, string>>();
            var @event = _fixture.Create<PostCreatedEvent>();
            DeliveryResult<string, string> deliveryResult = new DeliveryResult<string, string>();
            deliveryResult.Status = status;

            Mock.Get(_producerBuilder)
                .Setup(x => x.Build(_producerConfigOptions.Value))
                .Returns(producer);
            Mock.Get(producer)
                .Setup(x => x.ProduceAsync(_TOPIC_NAME, It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(deliveryResult);

            // Act
            var result = await _sut.ProduceAsync(_TOPIC_NAME, @event);

            // Assert
            result.Should().Be(status.ToEventPersistenceStatus());
        }

        [Theory]
        [InlineData(PersistenceStatus.NotPersisted)]
        public async Task ProduceAsync_Given_ShouldReturnsPersisted(PersistenceStatus status)
        {
            // Arrange
            var producer = Mock.Of<IProducer<string, string>>();
            var @event = _fixture.Create<PostCreatedEvent>();
            DeliveryResult<string, string> deliveryResult = new DeliveryResult<string, string>();
            deliveryResult.Status = status;

            Mock.Get(_producerBuilder)
                .Setup(x => x.Build(_producerConfigOptions.Value))
                .Returns(producer);
            Mock.Get(producer)
                .Setup(x => x.ProduceAsync(_TOPIC_NAME, It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(deliveryResult);

            // Act
            var ProduceAsyncAction = async () => await _sut.ProduceAsync(_TOPIC_NAME, @event);

            // Assert
            await ProduceAsyncAction
                .Should()
                .ThrowAsync<Exception>();
        }
    }
}