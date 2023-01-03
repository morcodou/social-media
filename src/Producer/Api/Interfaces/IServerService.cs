namespace Post.Command.Api.Interfaces
{
    public interface IServerService
    {
        DateTime Now { get; }
        string Version { get; }
    }
}