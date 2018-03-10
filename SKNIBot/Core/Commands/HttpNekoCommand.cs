using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup("Koty")]
    public class HttpNekoCommand
    {
        [Command("httpkot")]
        [Description("Składnia to '!httpkot {kod}'. Napisz '!httpkot kody' aby otrzymać listę dostępnych kodów.")]
        [Aliases("httpneko", "httpcat")]
        public async Task HttpNeko(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            //Jeżeli długość jest jeden nie podano kodu
            if (ctx.Message.Content.Split(' ').Length == 1)
            {
                await ctx.RespondAsync("Składnia to \'!httpkot {kod}\'. Napisz \'!httpkot kody\' aby otrzymać listę dostępnych kodów.");
            }
            //Jeżeli podano kod, sprawdzamy czy można otrzymać takiego kota
            else if(HttpNekoConst.Codes.Contains(ctx.Message.Content.Split(' ')[1]))
            {
                var client = new WebClient();
                var httpCatPicture = client.DownloadData("https://http.cat/" + ctx.Message.Content.Split(' ')[1]);
                var stream = new MemoryStream(httpCatPicture);

                await ctx.RespondWithFileAsync(stream, "httpneko.jpg");
            }
            //Jeżeli użytkownik prosi o kody kotów podajemy je
            else if(ctx.Message.Content.Split(' ')[1] == "kody")
            {
                var availableCodes = "";
                foreach(var code in HttpNekoConst.Codes)
                {
                    availableCodes += code;
                    availableCodes += " ";
                }

                await ctx.RespondAsync("Kody: " + availableCodes);
            }
            //Jeżeli nie można otrzymać kota informujemy o błędnym kodzie
            else
            {
                await ctx.RespondAsync("Błędny kod http. Napisz '!httpkot kody' aby otrzymać listę dostępnych kodów.");
            }
        }
    }
}
