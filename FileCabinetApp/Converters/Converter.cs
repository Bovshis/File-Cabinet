using System;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Converter.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Convert str to char.
        /// </summary>
        /// <param name="str">Converted string.</param>
        /// <returns>bool-is converted, string-conversion message, string-result.</returns>
        public static Tuple<bool, string, string> ConvertString(string str)
        {
            return new Tuple<bool, string, string>(true, "Done", str);
        }

        /// <summary>
        /// Convert str to char.
        /// </summary>
        /// <param name="date">Converted string.</param>
        /// <returns>bool-is converted, string-conversion message, DateTime-result.</returns>
        public static Tuple<bool, string, DateTime> ConvertDate(string date)
        {
            var dateParsed = DateTime.TryParse(date, out var dateTime);
            if (!dateParsed)
            {
                return new Tuple<bool, string, DateTime>(false, "Bad date format", dateTime);
            }

            return new Tuple<bool, string, DateTime>(true, "Done", dateTime);
        }

        /// <summary>
        /// Convert str to char.
        /// </summary>
        /// <param name="str">Converted string.</param>
        /// <returns>bool-is converted, string-conversion message, short-result.</returns>
        public static Tuple<bool, string, short> ConvertShort(string str)
        {
            var heightParsed = short.TryParse(str, out var height);
            if (!heightParsed)
            {
                return new Tuple<bool, string, short>(false, "Bad height format", height);
            }

            return new Tuple<bool, string, short>(true, "Done", height);
        }

        /// <summary>
        /// Convert str to char.
        /// </summary>
        /// <param name="str">Converted string.</param>
        /// <returns>bool-is converted, string-conversion message, decimal-result.</returns>
        public static Tuple<bool, string, decimal> ConvertDecimal(string str)
        {
            var weightParsed = decimal.TryParse(str, out var weight);
            if (!weightParsed)
            {
                return new Tuple<bool, string, decimal>(false, "Bad weight format", weight);
            }

            return new Tuple<bool, string, decimal>(true, "Done", weight);
        }

        /// <summary>
        /// Convert str to char.
        /// </summary>
        /// <param name="str">Converted string.</param>
        /// <returns>bool-is converted, string-conversion message, char-result.</returns>
        public static Tuple<bool, string, char> ConvertChar(string str)
        {
            var charParsed = char.TryParse(str, out var favoriteCharacter);
            if (!charParsed)
            {
                return new Tuple<bool, string, char>(false, "Bad char format", favoriteCharacter);
            }

            return new Tuple<bool, string, char>(true, "Done", favoriteCharacter);
        }

        /// <summary>
        /// Convert string to byte array.
        /// </summary>
        /// <param name="value">Converted string.</param>
        /// <param name="capacity">Capacity of byte array</param>
        /// <returns>byte array.</returns>
        public static byte[] StringToBytes(string value, int capacity)
        {
            var encoded = Encoding.UTF8.GetBytes(value);
            var byteArray = new byte[capacity];
            for (var i = 0; i < encoded.Length; i++)
            {
                byteArray[i] = encoded[i];
            }

            return byteArray;
        }
    }
}
