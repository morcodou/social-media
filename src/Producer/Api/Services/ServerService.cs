using Post.Command.Api.Interfaces;

namespace Post.Command.Api.Services
{
    public class ServerService : IServerService
    {
        private static string _version = "1.0.0";
        public DateTime Now => DateTime.Now;
        public string Version => _version;
    }
}