using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string Link { get; set; }

        public int ImageCategoryID { get; set; }
        public virtual ImageCategory ImageCategory { get; set; }

        public virtual IList<ImageName> Names { get; set; }
    }
}
