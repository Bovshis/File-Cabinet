using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class LastName
    {
        [JsonProperty("min")]
        public int MinValue { get; set; }

        [JsonProperty("max")]
        public int MaxValue { get; set; }
    }
}