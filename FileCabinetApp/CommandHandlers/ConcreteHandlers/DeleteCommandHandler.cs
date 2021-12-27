using System;
using System.Collections.Generic;
using System.Data;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var stringWithParameters = request.Parameters;
                    if (!stringWithParameters.StartsWith("where"))
                    {
                        throw new ArgumentException("parameters doesn't start with 'where'!");
                    }

                    var parameters = stringWithParameters
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

                    return $"Records #{string.Join(", #", this.service.Delete(list.ToArray()))} are deleted";
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }

            return base.Handle(request);
        }
    }
}
