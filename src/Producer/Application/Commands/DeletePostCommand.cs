namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class DeletePostCommand : BaseCommand
{
    public string Username { get; set; }= null!;
}