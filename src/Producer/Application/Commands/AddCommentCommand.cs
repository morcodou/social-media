namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class AddCommentCommand : BaseCommand
{
    public string Comment { get; set; } = null!;
    public string Username { get; set; } = null!;
}