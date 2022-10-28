using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public DateTime EditDate { get; set; }
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {
        }
    }
}