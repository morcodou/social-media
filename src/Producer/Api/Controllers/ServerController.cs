using Post.Command.Api.Interfaces;

namespace Post.Command.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServerController : CommandControllerBase<ServerController>
{
    private readonly IServerService _serverInformation;

    public ServerController(
        IServerService serverInformation,
        ILogger<ServerController> logger, ICommandDispatcher dispatcher)
        : base(logger, dispatcher) => _serverInformation = serverInformation;

    [HttpGet("date")]
    public ActionResult GetDate() => Ok(_serverInformation.Now);

    [HttpGet("version")]
    public ActionResult GetVersion() => Ok(_serverInformation.Version);
}