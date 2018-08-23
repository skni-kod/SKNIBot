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
        private const string CleverURL = "http://www.cleverbot.com/getreply?key={0}&input={1}&cs={2}";

        // talk
        private string _talkCs;

        // talk2
        private string[] _talk2Cs;
        private string _talk2NextMessage;
        private bool _talk2IsRunning;

        private const int MessagesPerSession = 20;

        public TalkCommand()
        {
            _talkCs = string.Empty;

            _talk2Cs = new string[2];
            _talk2NextMessage = string.Empty;
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
                    _talkCs = string.Empty;
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

        [Command("talk2")]
        [Description("Rozpocznij rozmowę dwóch bocików!")]
        [Aliases("tell2")]
        public async Task Talk2(CommandContext ctx, [Description("Wiadomość początkowa/`clear` - reset kontekstu/`stop` - zatrzymaj rozmowę")] string initialMessage = null)
        {
            if (initialMessage == "clear")
            {
                _talk2Cs[0] = "";
                _talk2Cs[1] = "";
                _talk2IsRunning = false;
                _talk2NextMessage = "";

                await ctx.RespondAsync("Kontekst zresetowany.");
            }
            else if (initialMessage == "stop")
            {
                _talk2IsRunning = false;
                await ctx.RespondAsync("Zatrzymano.");
            }
            else if (!_talk2IsRunning)
            {
                if (initialMessage != null)
                {
                    _talk2NextMessage = initialMessage;
                }

                RunTalk2(ctx);
            }
            else
            {
                await ctx.RespondAsync("Rozmowa ciągle trwa, wpisz tell2 stop aby ją zatrzymać.");
            }
        }

        private async void RunTalk2(CommandContext ctx)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            _talk2IsRunning = true;

            await ctx.RespondAsync("1: " + _talk2NextMessage);
            for (var i = 0; i < MessagesPerSession && _talk2IsRunning; i++)
            {
                await ctx.TriggerTypingAsync();

                var request = string.Format(CleverURL, SettingsLoader.Container.Clever_Key, _talk2NextMessage, _talk2Cs[i % 2]);
                var response = webClient.DownloadString(request);

                var talkData = JsonConvert.DeserializeObject<TalkData>(response);
                _talk2Cs[i % 2] = talkData.CS;
                _talk2NextMessage = talkData.Output;

                await ctx.RespondAsync(i % 2 + ": " + _talk2NextMessage);
            }

            _talk2IsRunning = false;
            await ctx.RespondAsync("`tell2` - kontynuuj rozmowę lub `tell2 clear` aby zresetować kontekst.");
        }

        private string GetResponseFromCleverBot(string message)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            var request = string.Format(CleverURL, SettingsLoader.Container.Clever_Key, message, _talkCs);
            var response = webClient.DownloadString(request);

            var talkData = JsonConvert.DeserializeObject<TalkData>(response);
            _talkCs = talkData.CS;

            return talkData.Output;
        }
    }
}
