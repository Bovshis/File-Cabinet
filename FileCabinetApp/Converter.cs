using System;

namespace FileCabinetApp
{
    public static class Converter
    {
        public static Tuple<bool, string, string> ConvertString(string str)
        {
            return new Tuple<bool, string, string>(true, "Done", str);
        }

        public static Tuple<bool, string, DateTime> ConvertDate(string date)
        {
            var dateParsed = DateTime.TryParse(date, out var dateTime);
            if (!dateParsed)
            {
                return new Tuple<bool, string, DateTime>(false, "Bad date format", dateTime);
            }

            return new Tuple<bool, string, DateTime>(true, "Done", dateTime);
        }

        public static Tuple<bool, string, short> ConvertShort(string str)
        {
            var heightParsed = short.TryParse(str, out var height);
            if (!heightParsed)
            {
                return new Tuple<bool, string, short>(false, "Bad height format", height);
            }

            return new Tuple<bool, string, short>(true, "Done", height);
        }

        public static Tuple<bool, string, decimal> ConvertDecimal(string str)
        {
            var weightParsed = decimal.TryParse(str, out var weight);
            if (!weightParsed)
            {
                return new Tuple<bool, string, decimal>(false, "Bad weight format", weight);
            }

            return new Tuple<bool, string, decimal>(true, "Done", weight);
        }

        public static Tuple<bool, string, char> ConvertChar(string str)
        {
            var charParsed = char.TryParse(str, out var favoriteCharacter);
            if (!charParsed)
            {
                return new Tuple<bool, string, char>(false, "Bad char format", favoriteCharacter);
            }

            return new Tuple<bool, string, char>(true, "Done", favoriteCharacter);
        }
    }
}
