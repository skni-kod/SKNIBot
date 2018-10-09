using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class TalkCommand : BaseCommandModule
    {
        [Command("talk")]
        [Description("Porozmawiaj ze mną!")]
        [Aliases("tell")]
        public async Task Talk(CommandContext ctx, [Description("Co chcesz mi powiedzieć? Wpisz 'clear' aby zresetować kontekst rozmowy.")] string message)
        {
            await ctx.RespondAsync("https://www.youtube.com/watch?v=V0PisGe66mY");
        }

        [Command("talk2")]
        [Description("Rozpocznij rozmowę dwóch bocików!")]
        [Aliases("tell2")]
        public async Task Talk2(CommandContext ctx, [Description("Wiadomość początkowa/`clear` - reset kontekstu/`stop` - zatrzymaj rozmowę")] string initialMessage = null)
        {
            await ctx.RespondAsync("https://www.youtube.com/watch?v=V0PisGe66mY");
        }
    }
}
