using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Commands.TextCommands.CleverModels;
using SKNIBot.Core.Containers.TextContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class TalkCommand : BaseCommandModule
    {
        private string Nick;

        public TalkCommand()
        {
            Nick = "";
        }

        [Command("talk")]
        [Description("Porozmawiaj ze mną!")]
        [Aliases("tell")]
        public async Task Talk(CommandContext ctx, [Description("Co chcesz mi powiedzieć? Wpisz 'clear' aby zresetować kontekst rozmowy.")] string message)
        {
            await ctx.TriggerTypingAsync();

            if (message == "clear")
            {
                Nick = await CreateBotInstance();
                await ctx.RespondAsync("Kontekst zresetowany.");
            }
            else
            {
                if (Nick == "")
                {
                    Nick = await CreateBotInstance();
                }

                var response = await SendTextAndGetReply(Nick, message);
                await ctx.RespondAsync(response);
            }
        }

        [Command("talk2")]
        [Description("Rozpocznij rozmowę dwóch bocików!")]
        [Aliases("tell2")]
        public async Task Talk2(CommandContext ctx, [Description("Wiadomość początkowa/`clear` - reset kontekstu/`stop` - zatrzymaj rozmowę")] string initialMessage = null)
        {
            await ctx.RespondAsync("https://www.youtube.com/watch?v=V0PisGe66mY");
        }

        private async Task<string> CreateBotInstance()
        {
            using (var httpClient = new HttpClient())
            {
                var createBotModel = new CreateBotModel
                {
                    User = SettingsLoader.Container.Clever_User,
                    Key = SettingsLoader.Container.Clever_Key
                };
                var createBotJson = JsonConvert.SerializeObject(createBotModel);
                var createBotContent = new StringContent(createBotJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://cleverbot.io/1.0/create", createBotContent);
                var responseJson = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<CreateBotResponseModel>(responseJson);
                return responseModel.Status == "success" ? responseModel.Nick : null;
            }
        }

        private async Task<string> SendTextAndGetReply(string nick, string text)
        {
            using (var httpClient = new HttpClient())
            {
                var queryBotModel = new QueryBotModel()
                {
                    User = SettingsLoader.Container.Clever_User,
                    Key = SettingsLoader.Container.Clever_Key,
                    Nick = nick,
                    Text = text
                };
                var queryBotJson = JsonConvert.SerializeObject(queryBotModel);
                var queryBotContent = new StringContent(queryBotJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://cleverbot.io/1.0/ask", queryBotContent);
                var responseJson = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<QueryBotResponseModel>(responseJson);
                return responseModel.Status == "success" ? responseModel.Response : null;
            }
        }
    }
}
