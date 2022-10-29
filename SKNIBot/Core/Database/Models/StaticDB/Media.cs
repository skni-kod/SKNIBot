using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models.StaticDB
{
    public class Media
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public bool IsDeleted { get; set; }

        public int CommandID { get; set; }
        public virtual Command Command { get; set; }

        public int CategoryID { get; set; }
        public virtual MediaCategory Category { get; set; }

        public virtual List<MediaName> Names { get; set; }
    }
}
