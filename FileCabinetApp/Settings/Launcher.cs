using System;
using System.IO;
using System.Text;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Settings
{
    public class Launcher
    {
        private StringBuilder logsMessage;

        public (IFileCabinetService service, string message) SetSettings(string[] settings)
        {
            try
            {
                this.logsMessage = new StringBuilder();
                var validator = this.SetValidationMode(settings);
                var storageService = this.SetStorageMode(settings, validator);
                var useStopWatch = this.UseStopWatch(settings, storageService);
                var logger = this.UseLogger(settings, useStopWatch);
                return (logger, this.logsMessage.ToString());
            }
            catch (Exception exception)
            {
                return (null, exception.Message);
            }
        }

        private IRecordValidator SetValidationMode(string[] settings)
        {
            var validationModeIndex = Array.FindIndex(settings, x => x is "--validation-rules" or "-v") + 1;
            if (validationModeIndex != 0)
            {
                const string validationRulesDefaultMode = "default", validationRulesCustomMode = "custom";
                var validationMode = settings[validationModeIndex];

                if (validationMode.Equals(validationRulesDefaultMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.logsMessage.Append("Using default validation rules.");
                    return new ValidatorBuilder().CreateDefault();
                }

                if (validationMode.Equals(validationRulesCustomMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.logsMessage.Append("Using custom validation rules.");
                    return new ValidatorBuilder().CreateCustom();
                }

                throw new ArgumentException($"Bad validation rules command");
            }

            this.logsMessage.Append("Using default validation rules.");
            return new ValidatorBuilder().CreateDefault();
        }

        private IFileCabinetService SetStorageMode(string[] settings, IRecordValidator validator)
        {
            var storageModeIndex = Array.FindIndex(settings, x => x is "--storage" or "-s") + 1;
            if (storageModeIndex != 0)
            {
                var storageMode = settings[storageModeIndex];
                const string memoryMode = "memory", fileMode = "file";

                if (storageMode.Equals(memoryMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.logsMessage.Append("Using memory cabinet.");
                    return new FileCabinetMemoryService(validator);
                }

                if (storageMode.Equals(fileMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.logsMessage.Append("Using file cabinet.");
                    return new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.Create), validator);
                }

                throw new ArgumentException("Bad validation rules command");
            }

            this.logsMessage.Append("Using memory cabinet.");
            return new FileCabinetMemoryService(validator);
        }

        private IFileCabinetService UseStopWatch(string[] settings, IFileCabinetService fileCabinetService)
        {
            var isUsed = Array.Exists(settings, x => x is "--use-stopwatch");
            if (isUsed)
            {
                this.logsMessage.Append("Using stop watch.");
                return new ServiceMeter(fileCabinetService);
            }

            return fileCabinetService;
        }

        private IFileCabinetService UseLogger(string[] settings, IFileCabinetService fileCabinetService)
        {
            var isUsed = Array.Exists(settings, x => x is "--use-logger");
            if (isUsed)
            {
                this.logsMessage.Append("Using logger.");
                return new ServiceLogger(fileCabinetService);
            }

            return fileCabinetService;
        }
    }
}
