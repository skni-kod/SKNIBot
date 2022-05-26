namespace SKNIBot.Core.Database.Models.DynamicDB
{
    public class UserMessageStat
    {
        public UserMessageStat()
        {
            count = 0;
            ChannelID = "Unknown";
            UserID = "Unknown";
        }
        public UserMessageStat(int cnt, string channel, string user)
         {
             count = cnt;
             ChannelID = channel;
             UserID = user;
         }
        public int ID { get; set; }
        public int ServerID { get; set; }
        public string ChannelID { get; set; }
        public string UserID { get; set; }
        public int count { get; set; }
        public virtual Server Server { get; set; }
    }
}
