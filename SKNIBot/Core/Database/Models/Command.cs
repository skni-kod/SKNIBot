using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
