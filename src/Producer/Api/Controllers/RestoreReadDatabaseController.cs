namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RestoreReadDatabaseController : ControllerBase
{
    private readonly ILogger<RestoreReadDatabaseController> _logger;
    private readonly ICommandDispatcher _dispatcher;
    public RestoreReadDatabaseController(
        ILogger<RestoreReadDatabaseController> logger,
        ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> RestoreReadDatabaseAsync()
    {
        try
        {
            var command = new RestoreReadDatabaseCommand();
            await _dispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new BaseResponse()
            {
                Message = "Restore read database request completed successfully!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to restore read database!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }
}