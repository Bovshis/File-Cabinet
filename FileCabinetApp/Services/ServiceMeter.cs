using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            var ticks = TickCounterService.GetTicks(this.service.CreateRecord, recordWithoutId, out var id);
            Console.WriteLine($"Create method execution duration is {ticks} ticks.");
            return id;
        }

        public void Insert(FileCabinetRecord record)
        {
            var ticks = TickCounterService.GetTicks(this.service.Insert, record);
            Console.WriteLine($"Insert method execution duration is {ticks} ticks.");
        }

        public bool EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            var ticks = TickCounterService.GetTicks(this.service.EditRecord, id, recordWithoutId, out var isEdited);
            Console.WriteLine($"Edit method execution duration is {ticks} ticks.");
            return isEdited;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var ticks = TickCounterService.GetTicks(this.service.FindByDateOfBirth, dateOfBirth, out var records);
            Console.WriteLine($"FindByDateOfBirth method execution duration is {ticks} ticks.");
            return records;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var ticks = TickCounterService.GetTicks(this.service.FindByFirstName, firstName, out var records);
            Console.WriteLine($"FindByFirstName method execution duration is {ticks} ticks.");
            return records;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var ticks = TickCounterService.GetTicks(this.service.FindByLastName, lastName, out var records);
            Console.WriteLine($"FindByLastName method execution duration is {ticks} ticks.");
            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetRecords, out var records);
            Console.WriteLine($"GetRecords method execution duration is {ticks} ticks.");
            return records;
        }

        public int GetStat()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetStat, out var stat);
            Console.WriteLine($"GetStat method execution duration is {ticks} ticks.");
            return stat;
        }

        public IRecordValidator GetValidator()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetValidator, out var validator);
            Console.WriteLine($"GetValidator method execution duration is {ticks} ticks.");
            return validator;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var ticks = TickCounterService.GetTicks(this.service.MakeSnapshot, out var snapshot);
            Console.WriteLine($"MakeSnapshot method execution duration is {ticks} ticks.");
            return snapshot;
        }

        public void Remove(int id)
        {
            var ticks = TickCounterService.GetTicks(this.service.Remove, id);
            Console.WriteLine($"MakeSnapshot method execution duration is {ticks} ticks.");
        }

        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot)
        {
            var ticks = TickCounterService.GetTicks(this.service.Restore, fileCabinetServiceSnapshot, out var amount);
            Console.WriteLine($"MakeSnapshot method execution duration is {ticks} ticks.");
            return amount;
        }
    }
}
