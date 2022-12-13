namespace Post.Query.Infrastructure.Handlers;

public class QueryEventHandler : IQueryEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public QueryEventHandler(
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task On(PostCreatedEvent @event)
    {
        var postEntity = new PostEntity()
        {
            PostId = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted,
            Message = @event.Message
        };

        await _postRepository.CreateAsync(postEntity);
    }

    public async Task On(PostLikedEvent @event)
    {
        var postEntity = await _postRepository.GetByIdAsync(@event.Id);
        if (postEntity == null) return;

        postEntity.Likes++;
        await _postRepository.UpdateAsync(postEntity);
    }

    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var commentEntity = new CommentEntity()
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            CommentDate = @event.CommentDate,
            Comment = @event.Comment,
            Username = @event.Username,
            Edited = false
        };
        await _commentRepository.CreateAsync(commentEntity);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var commentEntity = await _commentRepository.GetByIdAsync(@event.CommentId);
        if (commentEntity == null) return;

        commentEntity.Comment = @event.Comment;
        commentEntity.CommentDate = @event.EditDate;
        commentEntity.Edited = true;
        await _commentRepository.UpdateAsync(commentEntity);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var postEntity = await _postRepository.GetByIdAsync(@event.Id);
        if (postEntity == null) return;

        postEntity.Message = @event.Message;
        await _postRepository.UpdateAsync(postEntity);
    }
}