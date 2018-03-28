namespace SKNIBot.Core.Database.Models
{
    public class SimpleResponse
    {
        public int ID { get; set; }
        public string Content { get; set; }

        public int CommandID { get; set; }
        public virtual Command Command { get; set; }
    }
}
