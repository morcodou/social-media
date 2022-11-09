using CQRS.Core.Events;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override BaseEvent? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            
            if(!JsonDocument.TryParseValue(ref reader, out var documment))
            {
                throw new JsonException($"Failed to paser {nameof(JsonDocument)}");
            }
            if(!documment.RootElement.TryGetProperty("type", out var type))
            {
                throw new JsonException($"Could not detect the type discriminator property");
            }

            var typeDiscriminator = type.GetString();
            var jsonDocument = documment.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(jsonDocument, options),
                nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(jsonDocument, options),
                nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(jsonDocument, options),
                nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<MessageUpdatedEvent>(jsonDocument, options),
                nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(jsonDocument, options),
                nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<CommentRemovedEvent>(jsonDocument, options),
                nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(jsonDocument, options),
                _ => throw new JsonException($"{typeDiscriminator} is not supported yet")
            };
        }

        public override void Write(
            Utf8JsonWriter writer, 
            BaseEvent value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }
    }
}
