using System;
using Newtonsoft.Json;

namespace FileCabinetApp.Validators.Rules
{
    public class DateOfBirth
    {
        [JsonProperty("from")]
        public DateTime MinValue { get; set; }

        [JsonProperty("to")]
        public DateTime MaxValue { get; set; }
    }
}