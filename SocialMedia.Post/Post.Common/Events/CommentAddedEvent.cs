using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime CommentDate { get; set; }
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
        }
    }
}