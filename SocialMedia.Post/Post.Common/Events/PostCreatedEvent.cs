using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class PostCreatedEvent : BaseEvent
    {
        public string Author { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime DatePosted { get; set; }
        
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
        }
    }
}