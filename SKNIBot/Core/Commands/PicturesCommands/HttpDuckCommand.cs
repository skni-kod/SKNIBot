using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.PicturesConst;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class HttpDuckCommand : BaseCommandModule
    {
        [Command("httpduck")]
        [Description("Składnia to `!httpduck {kod}`. Napisz `!httpduck kody` aby otrzymać listę dostępnych kodów.")]
        [Aliases("httpkaczka")]
        public async Task HttpDuck(CommandContext ctx, [Description("Kod")] string code = null)
        {
            await ctx.TriggerTypingAsync();

            if (code == null)
            {
                await ctx.RespondAsync("Składnia to `!httpduck {kod}`. Napisz `!httpduck kody` aby otrzymać listę dostępnych kodów.");
            }
            //Jeżeli podano kod, sprawdzamy czy można otrzymać takiego kota
            else if (HttpDuckConst.Codes.Contains(code))
            {
                var client = new WebClient();

                await PostEmbedHelper.PostEmbed(ctx, "Duck!", code, $"https://random-d.uk/api/v1/http/{code}.jpg");
            }
            //Jeżeli użytkownik prosi o kody kotów podajemy je
            else if (code == "kody")
            {
                var builder = new StringBuilder();
                builder.Append("Kody: ");
                foreach (var c in HttpDuckConst.Codes)
                {
                    builder.Append($"{c} ");
                }

                await ctx.RespondAsync(builder.ToString());
            }
            //Jeżeli nie można otrzymać kota informujemy o błędnym kodzie
            else
            {
                await ctx.RespondAsync("Błędny kod http. Napisz `!httpduck kody` aby otrzymać listę dostępnych kodów.");
            }
        }
    }
}
