using System;
using System.Collections.Generic;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'update' command.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'update' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var stringWithParameters = request.Parameters;
            if (!stringWithParameters.StartsWith("set"))
            {
                throw new ArgumentException("Parameters doesn't start with 'set'!");
            }

            var parameters = stringWithParameters[4..].Split(" where ");
            if (parameters.Length != 2)
            {
                throw new ArgumentException("Wrong parameters");
            }

            var replaceList = GetReplaceParameters(parameters[0]);
            var whereList = GetWhereParameters(parameters[1]);
            var result = this.service.Update(replaceList, whereList);
            return result.Count == 0 ?
                "There is not records with such parameters."
                : $"Records #{string.Join(", #", result)} are updated";
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

        private static List<(string, string)> GetReplaceParameters(string parameters)
        {
            var replaceList = new List<(string, string)>();
            var replaceParameters = parameters
                .Replace(" ", string.Empty)
                .Split(",");
            foreach (var parameter in replaceParameters)
            {
                var parameterValue = parameter
                    .Split('=');
                if (parameterValue.Length != 2)
                {
                    throw new ArgumentException("Wrong parameters format");
                }

                replaceList.Add((parameterValue[0], parameterValue[1]));
            }

            return replaceList;
        }
    }
}
