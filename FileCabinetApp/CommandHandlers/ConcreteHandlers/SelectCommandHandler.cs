using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IList<FileCabinetRecord>, IList<string>> print;

        public SelectCommandHandler(IFileCabinetService service, Action<IList<FileCabinetRecord>, IList<string>> print)
            : base(service)
        {
            this.print = print;
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var parameters = request.Parameters.Split(" where ");

                    IList<FileCabinetRecord> records;
                    IList<string> printList;
                    if (parameters.Length is > 2 or 0)
                    {
                        throw new ArgumentException("Wrong parameters");
                    }
                    else if (parameters.Length == 2)
                    {
                        printList = GetPrintParameters(parameters[0]);
                        var whereList = GetWhereParameters(parameters[1]);
                        records = this.service.GetRecordsWhere(whereList);
                    }
                    else
                    {
                        printList = GetPrintParameters(parameters[0]);
                        records = this.service.GetRecords();
                    }

                    this.print(records, printList);
                    return "Completed";
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }

            return base.Handle(request);
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
