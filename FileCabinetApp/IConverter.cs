using System;

namespace FileCabinetApp
{
    public interface IConverter
    {
        Tuple<bool, string, DateTime> ConvertDate(string date);
        Tuple<bool, string, char> ConvertChar(string favoriteCharacterString);
        Tuple<bool, string, short> ConvertShort(string heightString);
        Tuple<bool, string, string> ConvertString(string str);
        Tuple<bool, string, decimal> ConvertDecimal(string weightString);
    }
}