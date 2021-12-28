namespace FileCabinetApp.Constants
{
    /// <summary>
    /// Constants for ByteRecord.
    /// </summary>
    public static class ByteOffsetConstants
    {
        /// <summary>
        /// Capacity byte representation of the name.
        /// </summary>
        public const ushort NameCapacity = 120;

        /// <summary>
        /// Capacity byte representation of the date.
        /// </summary>
        public const ushort DateCapacity = 12;

        /// <summary>
        /// Offset from start of recording to id.
        /// </summary>
        public const ushort IdOffset = 2;

        /// <summary>
        /// Offset from start of recording to FirstName.
        /// </summary>
        public const ushort FirstNameOffset = 6;

        /// <summary>
        /// Offset from start of recording to LastName.
        /// </summary>
        public const ushort LastNameOffset = 126;

        /// <summary>
        /// Offset from start of recording to Year.
        /// </summary>
        public const ushort YearOffset = 246;

        /// <summary>
        /// Offset from start of recording to Month.
        /// </summary>
        public const ushort MonthOffset = 250;

        /// <summary>
        /// Offset from start of recording to Day.
        /// </summary>
        public const ushort DayOffset = 254;

        /// <summary>
        /// Offset from start of recording to Height.
        /// </summary>
        public const ushort HeightOffset = 258;

        /// <summary>
        /// Offset from start of recording to Weight.
        /// </summary>
        public const ushort WeightOffset = 260;

        /// <summary>
        /// Offset from start of recording to FavoriteCharacter.
        /// </summary>
        public const ushort FavoriteCharacterOffset = 268;

        /// <summary>
        /// Total size byte representation of the record.
        /// </summary>
        public const ushort Size = 270;
    }
}
