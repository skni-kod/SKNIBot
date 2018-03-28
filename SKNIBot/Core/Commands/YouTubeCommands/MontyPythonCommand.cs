using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Helpers;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup("YouTube")]
    public class MontyPythonCommand
    {
        [Command("montypython")]
        [Description("Display random Monty Python.")]
        public async Task MontyPython(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                var video = databaseContext.Commands.First(p => p.Name == "MontyPython").SimpleResponses.Random();
                await ctx.RespondAsync(video.Content);
            }
        }
    }
}
