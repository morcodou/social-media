using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class MessageUpdatedEvent : BaseEvent
    {
        public string Message { get; set; } = null!;
        public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent))
        {
        }
    }
}