using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models
{
    public class HangmanCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<HangmanWord> Words { get; set; }
    }
}
