using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username { get; set; }
    }
}