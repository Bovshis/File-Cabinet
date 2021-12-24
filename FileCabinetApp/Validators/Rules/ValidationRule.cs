using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class ValidationRule
    {
        [JsonProperty("firstName")]
        public FirstName FirstName { get; set; }

        [JsonProperty("lastName")]
        public LastName LastName { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateOfBirth DateOfBirth { get; set; }

        [JsonProperty("height")]
        public Height Height { get; set; }

        [JsonProperty("weight")]
        public Weight Weight { get; set; }
    }
}