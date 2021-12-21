using System;
using System.Text;
using FileCabinetApp.Constants;
using FileCabinetApp.Converters;

namespace FileCabinetApp.Records
{
    /// <summary>
    /// Byte File Cabinet Record.
    /// </summary>
    public class ByteRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteRecord"/> class.
        /// </summary>
        /// <param name="fileCabinetRecord">file cabinet record for byte writing.</param>
        public ByteRecord(FileCabinetRecord fileCabinetRecord)
        {
            const short status = 1;
            this.Status = BitConverter.GetBytes(status);
            this.Id = BitConverter.GetBytes(fileCabinetRecord.Id);
            this.FirstName = Converter.StringToBytes(fileCabinetRecord.FirstName, ByteOffsetConstants.NameCapacity);
            this.LastName = Converter.StringToBytes(fileCabinetRecord.LastName, ByteOffsetConstants.NameCapacity);
            this.Year = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Year);
            this.Month = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Month);
            this.Day = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Day);
            this.Height = BitConverter.GetBytes(fileCabinetRecord.Height);
            this.Weight = BitConverter.GetBytes(decimal.ToDouble(fileCabinetRecord.Weight));
            this.FavoriteCharacter = BitConverter.GetBytes(fileCabinetRecord.FavoriteCharacter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteRecord"/> class.
        /// </summary>
        /// <param name="buffer">byte array that contains data for byte record.</param>
        public ByteRecord(byte[] buffer)
        {
            this.Status = buffer[..ByteOffsetConstants.IdOffset];
            this.Id = buffer[ByteOffsetConstants.IdOffset..ByteOffsetConstants.FirstNameOffset];
            this.FirstName = buffer[ByteOffsetConstants.FirstNameOffset..ByteOffsetConstants.LastNameOffset];
            this.LastName = buffer[ByteOffsetConstants.LastNameOffset..ByteOffsetConstants.YearOffset];
            this.Year = buffer[ByteOffsetConstants.YearOffset..ByteOffsetConstants.MonthOffset];
            this.Month = buffer[ByteOffsetConstants.MonthOffset..ByteOffsetConstants.DayOffset];
            this.Day = buffer[ByteOffsetConstants.DayOffset..ByteOffsetConstants.HeightOffset];
            this.Height = buffer[ByteOffsetConstants.HeightOffset..ByteOffsetConstants.WeightOffset];
            this.Weight = buffer[ByteOffsetConstants.WeightOffset..ByteOffsetConstants.FavoriteCharacterOffset];
            this.FavoriteCharacter = buffer[ByteOffsetConstants.FavoriteCharacterOffset..];
        }

        public byte[] Status { get; set; }

        public byte[] Id { get; set; }

        public byte[] FirstName { get; set; }

        public byte[] LastName { get; set; }

        public byte[] Year { get; set; }

        public byte[] Month { get; set; }

        public byte[] Day { get; set; }

        public byte[] Height { get; set; }

        public byte[] Weight { get; set; }

        public byte[] FavoriteCharacter { get; set; }

        /// <summary>
        /// Convert ByteRecord to FileCabinetRecord.
        /// </summary>
        /// <returns>file cabinet record.</returns>
        public FileCabinetRecord ToFileCabinetRecord()
        {
            return new FileCabinetRecord
            {
                Id = BitConverter.ToInt32(this.Id),
                FirstName = Encoding.UTF8.GetString(this.FirstName),
                LastName = Encoding.UTF8.GetString(this.LastName),
                DateOfBirth = new DateTime(
                    BitConverter.ToInt32(this.Year),
                    BitConverter.ToInt32(this.Month),
                    BitConverter.ToInt32(this.Day)),
                Height = BitConverter.ToInt16(this.Height),
                Weight = new decimal(BitConverter.ToDouble(this.Weight)),
                FavoriteCharacter = Encoding.UTF8.GetString(this.FavoriteCharacter)[0],
            };
        }
    }
}
