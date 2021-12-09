using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Record in the file cabinet.
    /// </summary>
    public class FileCabinetRecord
    {
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
