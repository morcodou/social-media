using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Enums;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;

namespace Post.Command.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IKafkaProducerBuilder _producerBuilder;
        public EventProducer(IKafkaProducerBuilder producerBuilder, IOptions<ProducerConfig> configuration)
        {
            _producerBuilder = producerBuilder;
            _producerConfig = configuration.Value;
        }

        public async Task<EventPersistenceStatus> ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = _producerBuilder.Build(_producerConfig);
            var eventMessage = new Message<string, string>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var deliveryResult = await producer.ProduceAsync(topic, eventMessage);
            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"Could not produce {@event.GetType().Name} message to topic {topic} du the followings reason {deliveryResult.Message}");
            }

            return deliveryResult.Status.ToEventPersistenceStatus();
        }
    }
}