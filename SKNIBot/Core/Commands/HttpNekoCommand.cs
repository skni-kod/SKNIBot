using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using SKNIBot.Core.Settings;
using Newtonsoft.Json;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class HttpNekoCommand
    {
        [Command("httpneko")]
        [Description("Display http codes using cats.")]
        [Aliases("httpcode")]
        public async Task HttpNeko(CommandContext ctx)
        {
            if(ctx.Message.Content.Split(' ').Length == 1)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync($"Składnia to '!httpneko *kod*' lub '!httpcode *kod*'");
            }
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync($"https://http.cat/" + ctx.Message.Content.Split(' ')[1]);
            }
            
        }
    }
}
