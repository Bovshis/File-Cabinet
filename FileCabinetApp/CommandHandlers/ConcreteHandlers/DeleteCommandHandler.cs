using System;
using System.Collections.Generic;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'export' command.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'delete' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var parameters = GetParameters(request.Parameters);
            return $"Records #{string.Join(", #", this.service.Delete(parameters))} are deleted";
        }

        private static (string, string)[] GetParameters(string str)
        {
            if (!str.StartsWith("where"))
            {
                throw new ArgumentException("parameters doesn't start with 'where'!");
            }

            var parameters = str
                .Replace("where", string.Empty)
                .Split(" and ");

            var list = new List<(string, string)>();
            foreach (var parameter in parameters)
            {
                var parameterValue = parameter
                    .Replace(" ", string.Empty)
                    .Split('=');
                if (parameterValue.Length != 2)
                {
                    throw new ArgumentException("Wrong parameters format");
                }

                list.Add((parameterValue[0], parameterValue[1]));
            }

            return list.ToArray();
        }
    }
}
