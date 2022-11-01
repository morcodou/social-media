using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
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

        public EventSourcingHandlerTest()
        {
            _eventStore = Mock.Of<IEventStore>();
            _sut = new(_eventStore);
        }
    }
}