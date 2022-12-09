using Confluent.Kafka;

namespace Post.Command.Infrastructure.Producers;

public class KafkaProducerBuilder : IKafkaProducerBuilder
{
    public IProducer<string, string> Build(ProducerConfig _producerConfig) =>
            new ProducerBuilder<string, string>(_producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();
}