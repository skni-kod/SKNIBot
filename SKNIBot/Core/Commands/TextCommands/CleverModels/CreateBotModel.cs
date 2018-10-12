using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SKNIBot.Core.Commands.TextCommands.CleverModels
{
    public class CreateBotModel
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
