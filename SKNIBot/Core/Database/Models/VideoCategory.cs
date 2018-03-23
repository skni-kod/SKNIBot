using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class VideoCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Video> Videos { get; set; }
    }
}
