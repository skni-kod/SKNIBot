using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    class InspirobotCommand : BaseCommandModule
    {
        [Command("memegen")]
        [Description("Wygeneruj mema z cytatem")]
        public async Task Inspirobot(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();

            var response = client.DownloadString("http://inspirobot.me/api?generate=true");            
            var answerPicture = client.DownloadData(response);
            var stream = new MemoryStream(answerPicture);

            await ctx.RespondAsync(new DiscordMessageBuilder().AddFile("answer.jpg", stream));
        }
    }
}
