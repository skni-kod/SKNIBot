using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup("YouTube")]
    public class MontyPythonCommand
    {
        private Random _random;

        public MontyPythonCommand()
        {
            _random = new Random();
        }

        [Command("montypython")]
        [Description("Display random Monty Python.")]
        public async Task MontyPython(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                var videoIndex = _random.Next(databaseContext.MontyPythonVideos.Count());
                var video = databaseContext.MontyPythonVideos.First(p => p.ID == videoIndex);

                await ctx.RespondAsync(video.Link);
            }
        }
    }
}
