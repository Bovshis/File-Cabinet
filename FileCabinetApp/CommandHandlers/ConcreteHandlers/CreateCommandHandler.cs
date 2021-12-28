using System;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'create' command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'create' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var id = this.service.CreateRecord(FileCabinetRecordConsoleReader.ReadRecordFromConsole(this.service.GetValidator()));
            return $"Record #{id} is created\n";
        }
    }
}
