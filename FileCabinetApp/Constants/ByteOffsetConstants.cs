namespace FileCabinetApp.Constants
{
    /// <summary>
    /// Constants for ByteRecord.
    /// </summary>
    public static class ByteOffsetConstants
    {
        public const ushort NameCapacity = 120;
        public const ushort DateCapacity = 12;
        public const ushort StatusOffset = 0;
        public const ushort IdOffset = 2;
        public const ushort FirstNameOffset = 6;
        public const ushort LastNameOffset = 126;
        public const ushort YearOffset = 246;
        public const ushort MonthOffset = 250;
        public const ushort DayOffset = 254;
        public const ushort HeightOffset = 258;
        public const ushort WeightOffset = 260;
        public const ushort FavoriteCharacterOffset = 268;
        public const ushort Size = 270;
    }
}
