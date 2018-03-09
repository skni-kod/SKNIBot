using System.Collections.Generic;

namespace SKNIBot.Core.Const
{
    public class HttpNekoConst
    {
        /// <summary>
        /// Lista kodów, które może zwrócić strona
        /// </summary>
        public static readonly List<string> Codes = new List<string>
        {
            "100", "101", "200", "201", "202", "204", "206", "207", "300",
            "301", "302", "303", "304", "305", "307", "400", "401", "402",
            "403", "404", "405", "406", "408", "409", "410", "411", "412",
            "413", "414", "415", "416", "417", "418", "420", "421", "422",
            "423", "424", "425", "426", "429", "431", "444", "450", "451",
            "500", "502", "503", "504", "506", "507", "508", "509", "510",
            "511", "599"
        };
    }
}
