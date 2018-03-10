using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup("Psy")]
    public class HttpInuCommand
    {
        [Command("httppies")]
        [Description("Składnia to '!httppies {kod}'. Napisz '!httppies kody' aby otrzymać listę dostępnych kodów.")]
        [Aliases("httpninu", "httpdog")]
        public async Task HttpInu(CommandContext ctx, [Description("Numer kodu")] string numer = null)
        {
            await ctx.TriggerTypingAsync();

            //Jeżeli długość jest jeden nie podano kodu
            if (numer == null)
            {
                await ctx.RespondAsync("Składnia to \'!httppies {kod}\'. Napisz \'!httppies kody\' aby otrzymać listę dostępnych kodów.");
            }
            //Jeżeli podano kod, sprawdzamy czy można otrzymać takiego psa
            else if (HttpInuConst.Codes.Contains(numer))
            {
                var client = new WebClient();
                var httpInuPicture = client.DownloadData("https://httpstatusdogs.com/img/" + ctx.Message.Content.Split(' ')[1] + ".jpg");
                var stream = new MemoryStream(httpInuPicture);
                
                await ctx.RespondWithFileAsync(stream, "httpinu.jpg");
            }
            //Jeżeli użytkownik prosi o kody ppsów podajemy je
            else if (numer == "kody")
            {
                var availableCodes = "";
                foreach (var code in HttpInuConst.Codes)
                {
                    availableCodes += code;
                    availableCodes += " ";
                }
                
                await ctx.RespondAsync("Kody: " + availableCodes);
            }
            //Jeżeli nie można otrzymać psa informujemy o błędnym kodzie
            else
            {
                await ctx.RespondAsync("Błędny kod http. Napisz '!httppies kody' aby otrzymać listę dostępnych kodów.");
            }
        }
    }
}
