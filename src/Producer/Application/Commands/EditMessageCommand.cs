namespace Post.Command.Application.Commands;

[ExcludeFromCodeCoverage]
public class EditMessageCommand : BaseCommand
{
    public string Message { get; set; } = null!;
}