using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Record in the file cabinet.
    /// </summary>
    [XmlRoot("record")]
    public class FileCabinetRecord
    {
        public FileCabinetRecord() {}

        public FileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth,
            short height, decimal weight, char favoriteCharacter)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Height = height;
            this.Weight = weight;
            this.FavoriteCharacter = favoriteCharacter;
        }

        /// <summary>
        /// Gets or sets record number in the file cabinet.
        /// </summary>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name in the record in the file cabinet.
        /// </summary>
        [XmlAttribute]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name in the record in the file cabinet.
        /// </summary>
        [XmlAttribute]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth in the record in the file cabinet.
        /// </summary>
        [XmlElement]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets height in the record in the file cabinet.
        /// </summary>
        [XmlElement]
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets weight in the record in the file cabinet.
        /// </summary>
        [XmlElement]
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets favorite character in the record in the file cabinet.
        /// </summary>
        [XmlElement]
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
