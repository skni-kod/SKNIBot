using System;

namespace SKNIBot.Core.Database.Models
{
    public class OnlineStats
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime LastOnline { get; set; }
        public int TotalTime { get; set; }
    }
}
