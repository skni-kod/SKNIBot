using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SKNIBot.Core.Containers.TextContainers.Covid19
{
    public class Covid19Location
    {
        public Covid19Coordinates Coordinates { get; set; }
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        public Dictionary<DateTime, int> History { get; set; }
        public int Latest { get; set; }
        public string Province { get; set; }
    }
}
