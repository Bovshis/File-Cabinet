using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class Weight
    {
        [JsonProperty("min")]
        public decimal MinValue { get; set; }

        [JsonProperty("max")]
        public decimal MaxValue { get; set; }
    }
}