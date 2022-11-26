using Post.Query.Infrastructure.Converters;
using System.Text.Json;
using CQRS.Core.Events;
using CQRS.Core.Messages;
using Post.Common.Events;

namespace Post.Query.Infrastructure.Test.Converters
{
    public class EventJsonConverterTest
    {
        private class NotRegisteredEvent : BaseEvent
        {
            public NotRegisteredEvent() : base(nameof(NotRegisteredEvent)) { }
        }

        private class NoTypePropertyEvent : Message
        {
            public int Version { get; set; }
        }

        private readonly EventJsonConverter _sut;
        private readonly JsonSerializerOptions _options;
        private readonly Fixture _fixture;
        public EventJsonConverterTest()
        {
            _sut = new();
            _options = new JsonSerializerOptions()
            {
                Converters = { _sut }
            };

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void EventJsonConverter_GivenPostCreatedEvent_ShouldDeserializeToPostCreatedEvent() => DeserializeTo<PostCreatedEvent>();

        [Fact]
        public void EventJsonConverter_GivenPostLikedEvent_ShouldDeserializeToPostLikedEvent() => DeserializeTo<PostLikedEvent>();

        [Fact]
        public void EventJsonConverter_GivenPostRemovedEvent_ShouldDeserializeToPostRemovedEvent() => DeserializeTo<PostRemovedEvent>();

        [Fact]
        public void EventJsonConverter_GivenMessageUpdatedEvent_ShouldDeserializeToMessageUpdatedEvent() => DeserializeTo<MessageUpdatedEvent>();

        [Fact]
        public void EventJsonConverter_GivenCommentAddedEvent_ShouldDeserializeToCommentAddedEvent() => DeserializeTo<CommentAddedEvent>();

        [Fact]
        public void EventJsonConverter_GivenCommentRemovedEvent_ShouldDeserializeToCommentRemovedEvent() => DeserializeTo<CommentRemovedEvent>();

        [Fact]
        public void EventJsonConverter_GivenCommentUpdatedEvent_ShouldDeserializeToCommentUpdatedEvent() => DeserializeTo<CommentUpdatedEvent>();

        [Fact]
        public void EventJsonConverter_GivenNotRegisteredEvent_ShouldThrowsJsonException()
        {
            // Arrange
            var typeDiscriminator = nameof(NoTypePropertyEvent);
            var @event = _fixture.Build<NotRegisteredEvent>()
                                 .With(x => x.Type, typeDiscriminator)
                                 .Create();
            var eventData = JsonSerializer.Serialize(@event);

            // Act
            var deserializeAction = () => JsonSerializer.Deserialize<BaseEvent>(eventData, _options);

            // Assert
            deserializeAction
                .Should()
                .Throw<JsonException>()
                .WithMessage($"{typeDiscriminator} is not supported yet");
        }

        [Fact]
        public void EventJsonConverter_GivenNoTypePropertyEvent_ShouldThrowsJsonException()
        {
            // Arrange
            var @event = _fixture.Create<NoTypePropertyEvent>();
            var eventData = JsonSerializer.Serialize(@event);

            // Act
            var deserializeAction = () => JsonSerializer.Deserialize<BaseEvent>(eventData, _options);

            // Assert
            deserializeAction
                .Should()
                .Throw<JsonException>()
                .WithMessage("Could not detect the type discriminator property");
        }

        private void DeserializeTo<TEvent>() where TEvent : BaseEvent
        {
            // Arrange
            var @event = _fixture.Build<TEvent>()
                                 .With(x => x.Type, typeof(TEvent).Name)
                                 .Create();
            var eventData = JsonSerializer.Serialize(@event);

            // Act
            var result = JsonSerializer.Deserialize<BaseEvent>(eventData, _options);

            // Assert
            result.Should().BeEquivalentTo(@event);
        }
    }
}