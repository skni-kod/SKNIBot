using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SKNIBot.Core.Containers.TextContainers.Covid19
{
    public class Covid19Stats
    {
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        public int Latest { get; set; }
        public string Source { get; set; }

        public IEnumerable<Covid19Location> Locations { get; set; }
    }
}
