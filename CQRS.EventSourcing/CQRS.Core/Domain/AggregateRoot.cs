using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id
        {
            get { return _id; }
            set { value = _id; }
        }
        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes;
        public void MarkChangesAsCommitted() => _changes.Clear();

        public void ApplyChanges(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}");
            }

            method.Invoke(this, new object[] { @event });
            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event) => ApplyChanges(@event, true);
        protected void ReplayEvents(IEnumerable<BaseEvent> events) 
        {
            foreach (var @event in events)
            {
                ApplyChanges(@event, false);
            }     
        }
    }
}