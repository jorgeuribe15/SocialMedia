using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Exceptions
{
    public  class AggregateNotFoundExceptions : Exception
    {
        public AggregateNotFoundExceptions(string message) : base(message)
        {

        }
    }
}
