using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models.StaticDB
{
    public class HangmanCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<HangmanWord> Words { get; set; }
    }
}
