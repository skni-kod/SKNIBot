using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class Video
    {
        public int ID { get; set; }
        public string Link { get; set; }

        public int VideoCategoryID { get; set; }
        public virtual VideoCategory VideoCategory { get; set; }

        public virtual List<VideoName> Names { get; set; }
    }
}
