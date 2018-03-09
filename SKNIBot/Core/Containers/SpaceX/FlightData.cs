using System;

namespace SKNIBot.Core.Containers.SpaceX
{
    public class FlightData
    {
        public int Flight_Number { get; set; }
        public DateTime Launch_Date_UTC { get; set; }
        public string Details { get; set; }

        public RocketData Rocket { get; set; }
        public LinksContainer Links { get; set; }
    }
}
