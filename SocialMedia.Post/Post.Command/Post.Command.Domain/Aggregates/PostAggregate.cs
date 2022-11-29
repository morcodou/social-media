using Post.Common.Events;

namespace Post.Command.Domain.Aggregates
{
    public sealed class PostAggregate : PostAggregateBase
    {
        private bool _active;
        private string _author = null!;

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {
        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent()
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public override void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent()
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event) => _id = @event.Id;

        public override void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent() { Id = _id });
        }

        public void Apply(PostLikedEvent @event) => _id = @event.Id;

        public override void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add comment to an inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidOperationException($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
            }

            RaiseEvent(new CommentAddedEvent()
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public override void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit comment of an inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidOperationException($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
            }

            if (!_comments.ContainsKey(commentId))
            {
                throw new InvalidOperationException($"You cannot edit a none existing comment");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Your are not allowed to edit a comment that was made by another user");
            }

            RaiseEvent(new CommentUpdatedEvent()
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public override void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove comment of an inactive post");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidOperationException($"The value of {nameof(username)} cannot be null or empty. Please provide a valid {nameof(username)}");
            }

            if (!_comments.ContainsKey(commentId))
            {
                throw new InvalidOperationException($"You cannot remove a none existing comment");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Your are not allowed to remove a comment that was made by another user");
            }

            RaiseEvent(new CommentRemovedEvent()
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public override void DeletePost(string author)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }

            if (!_author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Your are not allowed to delete a post that was made by another user");
            }

            RaiseEvent(new PostRemovedEvent() { Id = _id });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}