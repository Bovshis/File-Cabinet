using System;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="FileCabinetFilesystemService"/>.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'purge' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            if (this.service is not FileCabinetFilesystemService filesystemService)
            {
                throw new ArgumentException("It is not Filesystem storage\n");
            }

            var amount = filesystemService.Purge();
            return $"Data file processing is completed: {amount} records were purged.";
        }
    }
}
