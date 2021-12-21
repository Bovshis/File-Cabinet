using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    public class Generator
    {
        private string _formatType;
        private string _fileName;
        private int _amount;
        private int _startId;

        public void SetSettings(string[]? settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException();
            }

            if (settings.Length % 2 != 0)
            {
                throw new ArgumentException("Bad settings format!");
            }

            SetFormatType(settings);
            SetFileName(settings);
            SetAmount(settings);
            SetStartId(settings);
        }

        public void Generate()
        {
            if (_formatType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                GenerateToCsv();
                Console.WriteLine("generate data to csv");
            }
            else if (_formatType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                GenerateToXml();
                Console.WriteLine("generate data to xml");
            }
        }

        private void GenerateToCsv()
        {
            using var streamWriter = new StreamWriter(_fileName, true, System.Text.Encoding.Default);
            for (var i = _startId; i < _amount + _startId; i++)
            {
                streamWriter.WriteLine(GenerateFileCabinetRecord(i).ToString());
            }
        }

        private void GenerateToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
            var records = new List<FileCabinetRecord>();
            for (var i = _startId; i < _amount + _startId; i++)
            {
                records.Add(GenerateFileCabinetRecord(i));
            }

            using var fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fStream, records);
        }

        private FileCabinetRecord GenerateFileCabinetRecord(int i)
        {
            var allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            return new FileCabinetRecord()
            {
                Id = i,
                FirstName = RandomStrings(allowedChars, 2, 60),
                LastName = RandomStrings(allowedChars, 2, 60),
                DateOfBirth = RandomDate(new DateTime(1950, 1, 1), DateTime.Now),
                Height = (short)new Random().Next(0, 1000),
                Weight = new Random().Next(0, 1000),
                FavoriteCharacter = RandomChar(allowedChars),
            };
        }

        private void SetStartId(string[] settings)
        {
            var startIdIndex = Array.FindIndex(settings, x => x is "--start-id" or "-i") + 1;
            if (startIdIndex != 0)
            {
                var startId = int.Parse(settings[startIdIndex]);
                if (startId < 1)
                {
                    throw new ArgumentException("start id is less or equal than zero");
                }
                this._startId = startId;
            }
            else
            {
                throw new ArgumentException("The command doesn't contain start index setting!");
            }
        }

        private void SetAmount(string[] settings)
        {
            var amountIndex = Array.FindIndex(settings, x => x is "--records-amount" or "-a") + 1;
            if (amountIndex != 0)
            {
                var amount = int.Parse(settings[amountIndex]);
                if (amount < 1)
                {
                    throw new ArgumentException("amount is less or equal than zero");
                }
                this._amount = amount;
            }
            else
            {
                throw new ArgumentException("The command doesn't contain amount setting!");
            }
        }

        private void SetFileName(string[] settings)
        {
            var fileNameIndex = Array.FindIndex(settings, x => x is "--output" or "-o") + 1;
            if (fileNameIndex != 0)
            {
                var filePath = settings[fileNameIndex];
                var fileInfo = new FileInfo(filePath);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    throw new ArgumentException("Bad file path");
                }
                this._fileName = filePath;
            }
            else
            {
                throw new ArgumentException("The command doesn't contain filename setting!");
            }
        }

        private void SetFormatType(string[] settings)
        {
            var typeFileIndex = Array.FindIndex(settings, x => x is "--output-type" or "-t") + 1;
            if (typeFileIndex != 0)
            {
                var type = settings[typeFileIndex];
                if (type.Equals("csv", StringComparison.InvariantCultureIgnoreCase)
                    || type.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    this._formatType = settings[typeFileIndex];
                }
                else
                {
                    throw new ArgumentException("File type is not csv or xml");
                }
            }
            else
            {
                throw new ArgumentException("The command doesn't contain type setting!");
            }
        }

        private static char RandomChar(string allowedChars)
        {
            var rng = new Random();
            return allowedChars[rng.Next(0, allowedChars.Length - 1)];
        }

        private static DateTime RandomDate(DateTime minValue, DateTime maxValue)
        {
            var range = (maxValue - minValue).Days;
            return minValue.AddDays(new Random().Next(range));
        }

        private static string RandomStrings(string allowedChars, int minLength, int maxLength)
        {
            var rng = new Random();
            var chars = new char[maxLength];
            var setLength = allowedChars.Length;
            var length = rng.Next(minLength, maxLength + 1);
            for (var i = 0; i < length; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }

            return new string(chars, 0, length);
        }
    }
}
