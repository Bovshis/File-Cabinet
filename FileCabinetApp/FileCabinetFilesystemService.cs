using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord()
        {
            throw new System.NotImplementedException();
        }

        public void EditRecord(int id)
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public int GetStat()
        {
            throw new System.NotImplementedException();
        }
    }
}
