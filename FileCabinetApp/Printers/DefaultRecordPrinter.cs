using System;
using System.Collections.Generic;
using FileCabinetApp.Records;

namespace FileCabinetApp.Printers
{
    public static class DefaultRecordPrinter
    {
        public static void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var fileCabinetRecord in records)
            {
                Console.WriteLine(fileCabinetRecord.ToString());
            }
        }
    }
}
