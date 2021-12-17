using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for validation parameters.
    /// </summary>
    public interface IRecordValidator
    {
        Tuple<bool, string> ValidateFirstName(string firstname);

        Tuple<bool, string> ValidateLastName(string lastname);

        Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);

        Tuple<bool, string> ValidateHeight(short height);

        Tuple<bool, string> ValidateWeight(decimal weight);

        Tuple<bool, string> ValidateFavoriteCharacter(char favoriteCharacter);
    }
}
