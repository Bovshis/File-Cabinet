using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class Height
    {
        [JsonProperty("min")]
        public short MinValue { get; set; }

        [JsonProperty("max")]
        public short MaxValue { get; set; }
    }
}