namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Username { get; set; } = null!;
}