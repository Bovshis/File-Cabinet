using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'select' command.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IList<FileCabinetRecord>, IList<string>> print;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        /// <param name="print">Method for printing records.</param>
        public SelectCommandHandler(IFileCabinetService service, Action<IList<FileCabinetRecord>, IList<string>> print)
            : base(service)
        {
            this.print = print;
        }

        /// <summary>
        /// Execute 'select' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var parameters = request.Parameters.Split(" where ");

            IList<FileCabinetRecord> records;
            IList<string> printList;
            switch (parameters.Length)
            {
                case > 2 or 0:
                    throw new ArgumentException("Wrong parameters");
                case 2:
                    printList = GetPrintParameters(parameters[0]);
                    var whereList = GetWhereParameters(parameters[1]);
                    records = this.service.GetRecordsWhere(whereList);
                    break;
                default:
                    printList = GetPrintParameters(parameters[0]);
                    records = this.service.GetRecords();
                    break;
            }

            this.print(records, printList);
            return "Completed";
        }

        private static List<(string, string)> GetWhereParameters(string parameters)
        {
            var whereList = new List<(string, string)>();
            var whereParameters = parameters.Split(" and ");
            foreach (var parameter in whereParameters)
            {
                var parameterValue = parameter
                    .Replace(" ", string.Empty)
                    .Split('=');
                if (parameterValue.Length != 2)
                {
                    throw new ArgumentException("Wrong parameters format");
                }

                whereList.Add((parameterValue[0], parameterValue[1]));
            }

            return whereList;
        }

        private static IList<string> GetPrintParameters(string parameters)
        {
            return parameters
                .Replace(" ", string.Empty)
                .Split(",")
                .ToList();
        }
    }
}
