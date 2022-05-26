namespace SKNIBot.Core.Database.Models.DynamicDB
{
    public class DateMessageStat
    {
        public DateMessageStat()
        {
            count = 0;
            ChannelID = "Unknown";
            Date = "Unknown";
        }
        public DateMessageStat(int cnt, string channel, string date)
         {
             count = cnt;
             ChannelID = channel;
             Date = date;
         }
        public int ID { get; set; }
        public int ServerID { get; set; }
        public string ChannelID { get; set; }
        public string Date { get; set; }
        public int count { get; set; }
        public virtual Server Server { get; set; }
    }
}
