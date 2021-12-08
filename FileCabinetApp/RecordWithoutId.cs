using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Introduce Parameter Object.
    /// </summary>
    public class RecordWithoutId
    {
        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <value>not less than 0.</value>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets weightt.
        /// </summary>
        /// <value>not less than 0.</value>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets favorite character.
        /// </summary>
        public char FavoriteCharacter { get; set; }
    }
}
