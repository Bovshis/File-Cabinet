using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class FirstName
    {
        [JsonProperty("min")]
        public int MinValue { get; set; }

        [JsonProperty("max")]
        public int MaxValue { get; set; }
    }
}