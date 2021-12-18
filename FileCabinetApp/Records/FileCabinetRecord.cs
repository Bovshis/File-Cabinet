using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Record in the file cabinet.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// Create FileCabinetRecord from RecordWithoutId.
        /// </summary>
        /// <param name="id">record id.</param>
        /// <param name="recordWithoutId">record data.</param>
        public FileCabinetRecord(int id, RecordWithoutId recordWithoutId)
        {
            this.Id = id;
            this.FirstName = recordWithoutId.FirstName;
            this.LastName = recordWithoutId.LastName;
            this.DateOfBirth = recordWithoutId.DateOfBirth;
            this.Height = recordWithoutId.Height;
            this.Weight = recordWithoutId.Weight;
            this.FavoriteCharacter = recordWithoutId.FavoriteCharacter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord() {}

        /// <summary>
        /// Gets or sets record number in the file cabinet.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name in the record in the file cabinet.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name in the record in the file cabinet.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth in the record in the file cabinet.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets height in the record in the file cabinet.
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets weight in the record in the file cabinet.
        /// </summary>
        public decimal Weight { get; set; }

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
