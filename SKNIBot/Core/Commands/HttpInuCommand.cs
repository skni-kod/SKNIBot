using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class HttpInuCommand
    {
        [Command("httppies")]
        [Description("Składnia to '!httppies {kod}', '!httpninu {kod}' lub '!httpdog {kod}'. Napisz '!httppies kody', '!httpninu kody' lub '!httpdog kody' aby otrzymać listę dostępnych kodów.")]
        [Aliases("httpninu", "httpdog")]
        public async Task HttpInu(CommandContext ctx)
        {
            //Jeżeli długość jest jeden nie podano kodu
            if (ctx.Message.Content.Split(' ').Length == 1)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("Składnia to \'!httppies {kod}\', \'!httpninu {kod}\' lub \'!httpdog {kod}\'. Napisz \'!httppies kody\', \'!httpninu kody\' lub \'!httpdog kody\' aby otrzymać listę dostępnych kodów.");
            }
            //Jeżeli podano kod, sprawdzamy czy można otrzymać takiego psa
            else if (HttpInuConst.Codes.Contains(ctx.Message.Content.Split(' ')[1]))
            {
                var client = new WebClient();
                var httpInuPicture = client.DownloadData("https://httpstatusdogs.com/img/" + ctx.Message.Content.Split(' ')[1] + ".jpg");
                var stream = new MemoryStream(httpInuPicture);

                await ctx.TriggerTypingAsync();
                await ctx.RespondWithFileAsync(stream, "httpinu.jpg");
            }
            //Jeżeli użytkownik prosi o kody ppsów podajemy je
            else if (ctx.Message.Content.Split(' ')[1] == "kody")
            {
                var availableCodes = "";
                foreach (var code in HttpInuConst.Codes)
                {
                    availableCodes += code;
                    availableCodes += " ";
                }

                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("Kody: " + availableCodes);
            }
            //Jeżeli nie można otrzymać psa informujemy o błędnym kodzie
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("Błędny kod http. Napisz '!httppies kody', '!httpninu kody' lub '!httpdog kody' aby otrzymać listę dostępnych kodów.");
            }
        }
    }
}
