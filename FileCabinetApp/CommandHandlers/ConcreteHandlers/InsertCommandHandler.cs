using System;
using System.Globalization;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'insert' command.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'import' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var stringWithParameters = request.Parameters
                        .Replace(" ", string.Empty)
                        .Replace("(", string.Empty)
                        .Replace(")", string.Empty);
            const string keyword = "values";
            var parameters = stringWithParameters.Split(keyword);

            if (parameters.Length != 2)
            {
                throw new ArgumentException("Bad parameters format!");
            }

            const int keyIndex = 0;
            const int valuesIndex = 1;
            var keys = parameters[keyIndex].Split(",");
            var values = parameters[valuesIndex].Split(",");

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys count is not equals values count");
            }

            var record = new FileCabinetRecord();
            for (var i = 0; i < keys.Length; i++)
            {
                switch (keys[i].ToLower(CultureInfo.InvariantCulture))
                {
                    case "id":
                        record.Id = Convert.ToInt32(values[i]);
                        break;
                    case "firstname":
                        record.FirstName = values[i];
                        break;
                    case "lastname":
                        record.LastName = values[i];
                        break;
                    case "dateofbirth":
                        record.DateOfBirth = Convert.ToDateTime(values[i]);
                        break;
                    case "height":
                        record.Height = Convert.ToInt16(values[i]);
                        break;
                    case "weight":
                        record.Weight = Convert.ToDecimal(values[i]);
                        break;
                    case "favoritecharacter":
                        record.FavoriteCharacter = Convert.ToChar(values[i]);
                        break;
                    default:
                        throw new ArgumentException("Wrong key");
                }
            }

            var (isValidated, message) = this.service.GetValidator().ValidateParameter(record);
            if (!isValidated)
            {
                throw new ArgumentException(message);
            }

            this.service.Insert(record);
            return $"the record: {record} is inserted.";
        }
    }
}
