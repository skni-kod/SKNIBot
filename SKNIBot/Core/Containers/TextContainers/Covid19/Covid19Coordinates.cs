using Newtonsoft.Json;

namespace SKNIBot.Core.Containers.TextContainers.Covid19
{
    public class Covid19Coordinates
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("long")]
        public float Longitude { get; set; }
    }
}
