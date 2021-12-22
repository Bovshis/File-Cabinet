using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Records;

namespace FileCabinetApp.Printers
{
    public interface IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
