using CQRS.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Infrastructure
{

    /// <summary>
    /// Concrete Mediator
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// T is an in parameter
        /// Task is an out parameter        
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

        /// <summary>
        /// Using Liskow substitution - Concrete class should be sustituible by Base class
        /// </summary>
        Task SendAsync(BaseCommand command);


    }
}
