using System.IO;
using FileCabinetApp.Validators.Rules;
using Newtonsoft.Json;

namespace FileCabinetApp.Readers
{
    public static class ValidationRulesReader
    {
        public static ValidationRules ReadRulesFromConfig()
        {
            var jsonText = File.ReadAllText("validation-rules.json");
            var rules = JsonConvert.DeserializeObject<ValidationRules>(jsonText);
            return rules;
        }
    }
}
