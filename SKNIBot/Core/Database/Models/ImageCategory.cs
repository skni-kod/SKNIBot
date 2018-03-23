using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class ImageCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Image> Images { get; set; }
    }
}
