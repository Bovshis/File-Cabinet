using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public const int NumberOfParameters = 6;

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Height { get; set; }

        public decimal Weight { get; set; }

        public char FavoriteCharacter { get; set; }
    }
}
