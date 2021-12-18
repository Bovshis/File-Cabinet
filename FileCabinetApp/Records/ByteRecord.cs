using System;
using System.Text;

namespace FileCabinetApp.Records
{
    public class ByteRecord
    {
        private const ushort NameCapacity = 120;
        private const ushort StatusOffset = 0;
        private const ushort IdOffset = 2;
        private const ushort FirstNameOffset = 6;
        private const ushort LastNameOffset = 126;
        private const ushort YearOffset = 246;
        private const ushort MonthOffset = 250;
        private const ushort DayOffset = 254;
        private const ushort HeightOffset = 258;
        private const ushort WeightOffset = 260;
        private const ushort FavoriteCharacterOffset = 268;

        public ByteRecord(FileCabinetRecord fileCabinetRecord)
        {
            const short status = 1;
            this.Status = BitConverter.GetBytes(status);
            this.Id = BitConverter.GetBytes(fileCabinetRecord.Id);
            this.FirstName = ToBytes(fileCabinetRecord.FirstName, NameCapacity);
            this.LastName = ToBytes(fileCabinetRecord.LastName, NameCapacity);
            this.Year = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Year);
            this.Month = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Month);
            this.Day = BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Day);
            this.Height = BitConverter.GetBytes(fileCabinetRecord.Height);
            this.Weight = BitConverter.GetBytes(decimal.ToDouble(fileCabinetRecord.Weight));
            this.FavoriteCharacter = BitConverter.GetBytes(fileCabinetRecord.FavoriteCharacter);
        }

        public ByteRecord(byte[] buffer)
        {
            this.Status = buffer[..IdOffset];
            this.Id = buffer[IdOffset..FirstNameOffset];
            this.FirstName = buffer[FirstNameOffset..LastNameOffset];
            this.LastName = buffer[LastNameOffset..YearOffset];
            this.Year = buffer[YearOffset..MonthOffset];
            this.Month = buffer[MonthOffset..DayOffset];
            this.Day = buffer[DayOffset..HeightOffset];
            this.Height = buffer[HeightOffset..WeightOffset];
            this.Weight = buffer[WeightOffset..FavoriteCharacterOffset];
            this.FavoriteCharacter = buffer[FavoriteCharacterOffset..];
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

        private static byte[] ToBytes(string value, int capacity)
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
