namespace SKNIBot.Core.Database.Models
{
    public class ImageName
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int ImageID { get; set; }
        public virtual Image Image { get; set; }
    }
}
