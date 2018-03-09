using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.SpaceX
{
    public class FlightData
    {
        public int Flight_Number { get; set; }
        public DateTime Launch_Date_UTC { get; set; }
        public String Details { get; set; }

        public RocketData Rocket { get; set; }
        public LinksContainer Links { get; set; }
    }
}
