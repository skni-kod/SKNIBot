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
    public class TalkCommand
    {
        private const string CleverURL = "http://www.cleverbot.com/getreply?key={0}&input={1}&cs={2}";

        private string _cs;

        public TalkCommand()
        {
            _cs = string.Empty;
        }

        [Command("talk")]
        [Description("Porozmawiaj ze mną!")]
        [Aliases("tell")]
        public async Task Talk(CommandContext ctx, [Description("Co chcesz mi powiedzieć? Wpisz 'clear' aby zresetować kontekst rozmowy.")] string message)
        {
            await ctx.TriggerTypingAsync();

            switch (message)
            {
                case "clear":
                {
                    _cs = string.Empty;
                    await ctx.RespondAsync("Kontekst zresetowany.");

                    break;
                }

                default:
                {
                    await ctx.RespondAsync(GetResponseFromCleverBot(message));
                    break;
                }
            }
        }

        private string GetResponseFromCleverBot(string message)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            var request = string.Format(CleverURL, SettingsLoader.Container.Clever_Key, message, _cs);
            var response = webClient.DownloadString(request);

            var talkData = JsonConvert.DeserializeObject<TalkData>(response);
            _cs = talkData.CS;

            return talkData.Output;
        }
    }
}
