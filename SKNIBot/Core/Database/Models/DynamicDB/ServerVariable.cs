namespace SKNIBot.Core.Database.Models.DynamicDB
{
    public class ServerVariable
    {
        public ServerVariable()
        {
            ArchCounter = 0;
        }
        public ServerVariable(Server server)
        {
            Server = server;
            ArchCounter = 0;
        }
        public int ID { get; set; }
        public int ServerID { get; set; }
        public int ArchCounter { get; set; }
        public virtual Server Server { get; set; }
    }
}
