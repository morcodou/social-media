namespace Post.Command.Application.Interfaces;

public interface ICommandDispatcher
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
    Task SendAsync(BaseCommand command);
}