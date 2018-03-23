namespace SKNIBot.Core.Database.Models
{
    public class VideoName
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int VideoID { get; set; }
        public virtual Video Video { get; set; }
    }
}
