using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Records;
using FileCabinetApp.Writers;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const ushort ByteRecordSize = 270;
        private int recordsAmount = 0;
        private readonly FileStream fileStream;

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            this.recordsAmount++;
            var byteRecord = new ByteRecord(new FileCabinetRecord(this.recordsAmount, recordWithoutId));
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
            return this.recordsAmount;
        }

        public void EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            long offset = ByteRecordSize * (id - 1);
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            var byteRecord = new ByteRecord(new FileCabinetRecord(id, recordWithoutId));
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {

        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var records = new List<FileCabinetRecord>();

            var currentIndex = 0;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            while (currentIndex < this.fileStream.Length)
            {
                var buffer = new byte[ByteRecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                var byteRecord = new ByteRecord(buffer);
                records.Add(byteRecord.ToFileCabinetRecord());
                currentIndex += ByteRecordSize;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records.ToArray());
        }

        public int GetStat()
        {
            return this.recordsAmount;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new System.NotImplementedException();
        }
    }
}
