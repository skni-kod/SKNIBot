using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Database.Models
{
    public class ImageCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Image> Images { get; set; }
    }
}
