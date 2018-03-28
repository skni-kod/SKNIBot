namespace SKNIBot.Core.Database.Models
{
    public class MediaName
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int MediaID { get; set; }
        public virtual Media Media { get; set; }
    }
}
