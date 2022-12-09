namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class EditCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; } = null!;
    public string Username { get; set; } = null!;
}