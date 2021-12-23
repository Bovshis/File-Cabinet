using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class ValidationRules
    {
        [JsonProperty("default")]
        public ValidationRule Default { get; set; }

        [JsonProperty("custom")]
        public ValidationRule Custom { get; set; }
    }
}