using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public const int NumberOfParameters = 7;

        private string firstName;
        private string lastname;
        private DateTime dateOfBirth;
        private short height;
        private decimal weight;

        public int Id { get; set; }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{nameof(value)} is WhiteSpace", nameof(value));
                }

                if (value.Length < 2 || value.Length > 60)
                {
                    throw new ArgumentException($"{nameof(value)} lenght less than 2 or greater than 60", nameof(value));
                }

                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastname;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{nameof(value)} is WhiteSpace", nameof(value));
                }

                if (value.Length < 2 || value.Length > 60)
                {
                    throw new ArgumentException($"{nameof(value)} lenght less than 2 or greater than 60", nameof(value));
                }

                this.lastname = value;
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }

            set
            {
                if (value < new DateTime(1950, 1, 1) || value > DateTime.Now)
                {
                    throw new ArgumentException($"{nameof(value)} is less than 1 Jan 1950 or greater than current time", nameof(value));
                }

                this.dateOfBirth = value;
            }
        }

        public short Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"{nameof(value)} is less than 0", nameof(value));
                }

                this.height = value;
            }
        }

        public decimal Weight
        {
            get
            {
                return this.weight;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"{nameof(value)} is less than 0", nameof(value));
                }

                this.weight = value;
            }
        }

        public char FavoriteCharacter { get; set; }
    }
}
