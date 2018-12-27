using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SKNIBot.Core.Commands.TextCommands.CleverModels
{
    public class QueryBotModel
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
