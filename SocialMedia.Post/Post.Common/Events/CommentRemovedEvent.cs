using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentRemovedEvent : BaseEvent
    {
        public Guid CommentId { get; set; }
        public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
        {
        }
    }
}