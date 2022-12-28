using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using System.Text.Json;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IQueryEventHandler _eventHandler;

    public EventConsumer(IOptions<ConsumerConfig> consumerConfig, IQueryEventHandler eventHandler)
    {
        _consumerConfig = consumerConfig.Value;
        _eventHandler = eventHandler;
    }

    public void Consume(string topic)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult?.Message == null) continue;

            var options = new JsonSerializerOptions()
            {
                Converters = { new EventJsonConverter() }
            };

            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
            var types = new List<Type>();
            if (@event != null)
                types.Add(@event.GetType());

            var handlerMethod = _eventHandler.GetType().GetMethod("On", types.ToArray());
            if (handlerMethod == null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "Could not found event handler method");
            }

            handlerMethod.Invoke(_eventHandler, new object[] { @event! });
            consumer.Commit(consumeResult);
        }
    }
}