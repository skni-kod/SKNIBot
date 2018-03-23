using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
