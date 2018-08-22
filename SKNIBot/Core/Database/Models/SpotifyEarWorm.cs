using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Database.Models
{
    public enum TypeOfPeriod {DAY, WEEK, MONTH, YEAR}
    public class SpotifyEarWorm
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }
        public String Uri { get; set; }
        public DateTime DayStamp { get; set; }
        public TypeOfPeriod Category { get; set; }
    }
}
