using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.Dtos;
using Post.Common.Dtos;

namespace Post.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeletePostController : ControllerBase
    {
        private readonly ILogger<DeletePostController> _logger;
        private readonly ICommandDispatcher _dispatcher;
        public DeletePostController(
            ILogger<DeletePostController> logger,
            ICommandDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePostAsync(Guid id, DeletePostCommand command)
        {
            try
            {   
                command.Id = id;
                await _dispatcher.SendAsync(command);

                return Ok(new BaseResponse()
                {
                    Message = "Remove post request completed successfully!"
                });
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogWarning(exception, "Client made a bad request!");
                return BadRequest(new BaseResponse()
                {
                    Message = exception.Message
                });
            }
            catch (AggregateNotFoundException exception)
            {
                _logger.LogWarning(exception, "Could not retrieve aggregate, client passe incorrect post Id targeting the aggregate!");
                return BadRequest(new BaseResponse()
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                var errorMessage = "An error occurred while processing request to remove post!";
                _logger.LogError(exception, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse()
                {
                    Id = id,
                    Message = errorMessage
                });
            }
        }
    }
}