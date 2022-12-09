using Confluent.Kafka;

namespace Post.Command.Infrastructure.Producers;

public interface IKafkaProducerBuilder
{
    IProducer<string, string> Build(ProducerConfig _producerConfig);
}