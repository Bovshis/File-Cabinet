using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Records;

namespace FileCabinetApp.Printers
{
    /// <summary>
    /// Record printer.
    /// </summary>
    public static class DefaultRecordPrinter
    {
        /// <summary>
        /// Print records to console.
        /// </summary>
        /// <param name="records">Records for printing.</param>
        /// <param name="fields">Fields that print.</param>
        public static void Print(IList<FileCabinetRecord> records, IList<string> fields)
        {
            var lengths = GetLengths(records, fields);
            var separatingString = GetSeparatingString(lengths);
            Console.WriteLine(separatingString);
            Console.WriteLine(GetHead(lengths, fields));
            foreach (var fileCabinetRecord in records)
            {
                Console.WriteLine(separatingString);
                Console.WriteLine(GetNewLine(lengths, fields, fileCabinetRecord));
            }

            Console.WriteLine(separatingString);
        }

        private static string GetHead(int[] lengths, IList<string> fields)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < lengths.Length; i++)
            {
                sb.Append("| ");
                var value = fields[i].ToLower(CultureInfo.InvariantCulture) switch
                {
                    "id" => "Id".PadLeft(lengths[i]),
                    "firstname" => "FirstName".PadRight(lengths[i]),
                    "lastname" => "LastName".PadRight(lengths[i]),
                    "dateofbirth" => "DateOfBirth".PadRight(lengths[i]),
                    "height" => "Height".PadLeft(lengths[i]),
                    "weight" => "Weight".PadLeft(lengths[i]),
                    "favoritecharacter" => "FavoriteCharacter".PadRight(lengths[i]),
                    _ => throw new ArgumentException($"There is no field: {fields[i]}"),
                };
                sb.Append(value).Append(' ');
            }

            sb.Append('|');
            return sb.ToString();
        }

        private static string GetNewLine(int[] lengths, IList<string> fields, FileCabinetRecord fileCabinetRecord)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < lengths.Length; i++)
            {
                sb.Append("| ").Append(GetNewValue(lengths[i], fields[i], fileCabinetRecord)).Append(' ');
            }

            sb.Append('|');
            return sb.ToString();
        }

        private static string GetNewValue(int length, string field, FileCabinetRecord fileCabinetRecord)
        {
            return field.ToLower(CultureInfo.InvariantCulture) switch
            {
                "id" => fileCabinetRecord.Id.ToString().PadLeft(length),
                "firstname" => fileCabinetRecord.FirstName.PadRight(length),
                "lastname" => fileCabinetRecord.LastName.PadRight(length),
                "dateofbirth" => fileCabinetRecord.DateOfBirth.ToString("yyyy-MMM-dd").PadRight(length),
                "height" => fileCabinetRecord.Height.ToString().PadLeft(length),
                "weight" => fileCabinetRecord.Weight.ToString(CultureInfo.InvariantCulture).PadLeft(length),
                "favoritecharacter" => fileCabinetRecord.FavoriteCharacter.ToString().PadRight(length),
                _ => throw new ArgumentException($"There is no field: {field}"),
            };
        }

        private static string GetSeparatingString(int[] lengths)
        {
            var sb = new StringBuilder();
            foreach (var length in lengths)
            {
                sb.Append('+').Append(' ').Append(string.Empty.PadLeft(length, '-')).Append(' ');
            }

            sb.Append('+');
            return sb.ToString();
        }

        private static int[] GetLengths(IList<FileCabinetRecord> records, IList<string> fields)
        {
            var lengths = new int[fields.Count];
            for (var i = 0; i < fields.Count; i++)
            {
                lengths[i] = GetMaxLength(fields[i], records);
            }

            return lengths;
        }

        private static int GetMaxLength(string field, IList<FileCabinetRecord> records)
        {
            return field.ToLower(CultureInfo.InvariantCulture) switch
            {
                "id" => Math.Max(field.Length, records.Select(x => x.Id.ToString().Length).Max()),
                "firstname" => Math.Max(field.Length, records.Select(x => x.FirstName.Length).Max()),
                "lastname" => Math.Max(field.Length, records.Select(x => x.LastName.Length).Max()),
                "dateofbirth" => Math.Max(field.Length, records.Select(x => x.DateOfBirth.ToString("yyyy-MMM-dd").Length).Max()),
                "height" => Math.Max(field.Length, records.Select(x => x.Height.ToString().Length).Max()),
                "weight" => Math.Max(field.Length, records.Select(x => x.Weight.ToString(CultureInfo.InvariantCulture).Length).Max()),
                "favoritecharacter" => field.Length,
                _ => throw new ArgumentException($"There is no field: {field}"),
            };
        }
    }
}
