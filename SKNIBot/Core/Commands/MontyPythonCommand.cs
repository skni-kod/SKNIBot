using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class MontyPythonCommand
    {
        private List<string> _videos;
        private Random _random;

        private const string _videosFile = "montypython.json";

        public MontyPythonCommand()
        {
            _random = new Random();

            using (var file = new StreamReader(_videosFile))
            {
                _videos = JsonConvert.DeserializeObject<List<string>>(file.ReadToEnd());
            }
        }

        [Command("montypython")]
        [Description("Display random Monty Python.")]
        public async Task MontyPython(CommandContext ctx)
        {
            var videoIndex = _random.Next(_videos.Count);

            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(_videos[videoIndex]);
        }
    }
}
