using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class Command
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<SimpleResponse> SimpleResponses { get; set; }
        public virtual List<Media> Media { get; set; }
    }
}
