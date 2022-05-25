namespace SKNIBot.Core.Database.Models.DynamicDB
{
    public class UserMessageStat
    {
        public UserMessageStat()
        {
            count = 0;
            ChannelID = 0;
            User = 0;
        }
        public UserMessageStat(int cnt, ulong channel, ulong user)
         {
             count = cnt;
             ChannelID = channel;
             User = user;
         }
        public int ID { get; set; }
        public int ServerID { get; set; }
        public ulong ChannelID { get; set; }
        public ulong User { get; set; }
        public int count { get; set; }
        public virtual Server Server { get; set; }
    }
}
