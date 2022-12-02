using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Post.Command.Api.Controllers
{
    public abstract class CommandControllerBase<TController> : ControllerBase
    {
        protected readonly ILogger<TController> _logger;
        protected readonly ICommandDispatcher _dispatcher;
        public CommandControllerBase(ILogger<TController> logger, ICommandDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }
    }
}