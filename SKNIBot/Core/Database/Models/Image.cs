using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Database.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string Link { get; set; }

        public int CategoryID { get; set; }
        public virtual ImageCategory Category { get; set; }

        public virtual IList<ImageName> Names { get; set; }
    }
}
