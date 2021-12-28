using System;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'exit' command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private readonly Action<bool> exit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="exit">Method than change isRunning <see cref="Program"/>.</param>
        public ExitCommandHandler(Action<bool> exit)
        {
            this.exit = exit;
        }

        /// <summary>
        /// Execute 'exit' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            this.exit(false);
            return "Exiting an application...\n";
        }
    }
}
