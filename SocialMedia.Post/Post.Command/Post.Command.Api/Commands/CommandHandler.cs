using CQRS.Core.Handlers;
using Post.Command.Api.Interfaces.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Domain.Factories;

namespace Post.Command.Api.Commands
{
    public class CommandHandler<TPostAggregate> : ICommandHandler where TPostAggregate :
        PostAggregateBase
    {
        public IEventSourcingHandler<TPostAggregate> _eventSourcingHandler { get; }
        public IPostAggregateFactory<TPostAggregate> _postAggregateFactory { get; }

        public CommandHandler(IEventSourcingHandler<TPostAggregate> eventSourcingHandler,
                              IPostAggregateFactory<TPostAggregate> postAggregateFactory)
        {
            _eventSourcingHandler = eventSourcingHandler;
            _postAggregateFactory = postAggregateFactory;
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = _postAggregateFactory.Create(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}