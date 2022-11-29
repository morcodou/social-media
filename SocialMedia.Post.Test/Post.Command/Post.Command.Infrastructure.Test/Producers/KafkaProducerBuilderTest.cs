using Confluent.Kafka;
using Post.Command.Infrastructure.Producers;

namespace Post.Command.Infrastructure.Test.Producers
{
    public class KafkaProducerBuilderTest
    {
        private readonly KafkaProducerBuilder _sut = new();

        [Fact]
        public void Build_GivenConfig_ShouldReturnsProducer()
        {
            // Arrange
            var producerConfig = new ProducerConfig() { BootstrapServers = "127.0.0.1:9092" };

            // Act
            var result = _sut.Build(producerConfig);

            // Assert
            result.Should().BeAssignableTo<IProducer<string, string>>();
        }
    }
}