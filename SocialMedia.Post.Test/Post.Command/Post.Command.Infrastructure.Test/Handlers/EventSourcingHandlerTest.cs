using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Command.Infrastructure.Handlers;

namespace Post.Command.Infrastructure.Test.Handlers
{
    public class EventSourcingHandlerTest
    {
        private class FakePostAggregate : AggregateRoot
        {
            public FakePostAggregate()
            {
            }
        }

        private readonly EventSourcingHandler<FakePostAggregate> _sut;
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandlerTest()
        {
            _eventStore = Mock.Of<IEventStore>();
            _eventProducer = Mock.Of<IEventProducer>();
            _sut = new(_eventStore, _eventProducer);
        }
    }
}