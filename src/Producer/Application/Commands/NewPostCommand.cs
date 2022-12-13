namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class NewPostCommand : BaseCommand
{
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
}