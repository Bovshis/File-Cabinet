using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for storing record in the file cabinet.
    /// </summary>
    public class FileCabinetRecord
    {
        private string firstName;
        private string lastname;
        private DateTime dateOfBirth;
        private short height;
        private decimal weight;

        /// <summary>
        /// Gets or sets record number in the file cabinet.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name in the record in the file cabinet.
        /// </summary>
        /// <value>not null or whitespace, not less than 2 or not greater than 60.</value>
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

        /// <summary>
        /// Gets or sets last name in the record in the file cabinet.
        /// </summary>
        /// <value>not null or whitespace, not less than 2 or not greater than 60.</value>
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

        /// <summary>
        /// Gets or sets date of birth in the record in the file cabinet.
        /// </summary>
        /// <value>not less than 1950-Jan-1 or not greater than current time.</value>
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

        /// <summary>
        /// Gets or sets height in the record in the file cabinet.
        /// </summary>
        /// <value>not less than 0.</value>
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

        /// <summary>
        /// Gets or sets weight in the record in the file cabinet.
        /// </summary>
        /// <value>not less than 0.</value>
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

        /// <summary>
        /// Gets or sets favorite character in the record in the file cabinet.
        /// </summary>
        public char FavoriteCharacter { get; set; }

        /// <summary>
        /// Get record in string format.
        /// </summary>
        /// <returns>
        /// return record in string format.
        /// </returns>
        public override string ToString() => $"#{this.Id}, {this.FirstName}, {this.LastName}, {this.DateOfBirth:yyyy-MMM-dd}, " +
                        $"{this.Height}, {this.Weight}, {this.FavoriteCharacter}";
    }
}
