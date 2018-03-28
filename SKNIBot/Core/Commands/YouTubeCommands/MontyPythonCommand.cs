using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
                var montyPythonVideos = databaseContext.SimpleResponses.Where(p => p.Command.Name == "MontyPython");
                var randomIndex = _random.Next(montyPythonVideos.Count());

                var videoLink = montyPythonVideos
                    .OrderBy(p => p.ID)
                    .Select(p => p.Content)
                    .Skip(randomIndex)
                    .First();

                await ctx.RespondAsync(videoLink);
            }
        }
    }
}
