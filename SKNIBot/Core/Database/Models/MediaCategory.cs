using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class MediaCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Media> Media { get; set; }
    }
}
