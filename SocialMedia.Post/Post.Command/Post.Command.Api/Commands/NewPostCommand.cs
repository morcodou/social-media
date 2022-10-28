using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class NewPostCommand : BaseCommand
    {
        public string Author { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}