using System.IO;
using FileCabinetApp.Validators.Rules;
using Newtonsoft.Json;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Validation rules reader.
    /// </summary>
    public static class ValidationRulesReader
    {
        /// <summary>
        /// Read rules from config file.
        /// </summary>
        /// <returns>Validation rules <see cref="ValidationRules"/>.</returns>
        public static ValidationRules ReadRulesFromConfig()
        {
            var jsonText = File.ReadAllText("validation-rules.json");
            var rules = JsonConvert.DeserializeObject<ValidationRules>(jsonText);
            return rules;
        }
    }
}
