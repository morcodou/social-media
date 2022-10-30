using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Core.Exceptions
{
    public class AggragateNotFoundException : Exception
    {
        public AggragateNotFoundException(string message) : base(message)
        {

        }
    }
}