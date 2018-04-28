using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.CompilationContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class CompileCommand
    {
        private const string ApiEndpoint = "https://api.jdoodle.com/v1/execute";

        [Command("kompiluj")]
        [Description("Kompiluje dany kod źródłowy i wyświetla wynik. Składnia: !kompiluj [język], [wejście], [kod]. Lista języków pod adresem https://www.jdoodle.com/compiler-api/docs")]
        [Aliases("compile", "uruchom", "run")]
        public async Task Compile(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            string language = "", input = "", code = "";

            var splittedInput = ctx.RawArgumentString.Split(',').ToList();
            switch (splittedInput.Count)
            {
                case 2:
                {
                    language = splittedInput[0].Trim();
                    input = "";
                    code = splittedInput[1].Trim();

                    break;
                    }
                case 3:
                {
                    language = splittedInput[0].Trim();
                    input = splittedInput[1].Trim();
                    code = splittedInput[2].Trim();

                    break;
                }
            }

            var compilationResult = GetCompilationResult(language, input, code);
            var compilationEmbedResult = GetEmbedResult(compilationResult);

            await ctx.RespondAsync("", false, compilationEmbedResult);
        }

        private CompilationResultContainer GetCompilationResult(string language, string input, string code)
        {
            var compileRequest = new CompileRequestContainer
            {
                clientId = SettingsLoader.Container.JDoodle_Client_ID,
                clientSecret = SettingsLoader.Container.JDoodle_Client_Secret,
                language = language,
                script = code,
                stdin = input,
                versionIndex = "0"
            };

            var compileRequestJSON = JsonConvert.SerializeObject(compileRequest);

            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var response = webClient.UploadString(ApiEndpoint, compileRequestJSON);

            return JsonConvert.DeserializeObject<CompilationResultContainer>(response);
        }

        private DiscordEmbed GetEmbedResult(CompilationResultContainer compilationResult)
        {
            var output = compilationResult.output == "" ? "Brak" : compilationResult.output;

            var embedResult = new DiscordEmbedBuilder();
            embedResult.AddField("Rezultat", output);

            return embedResult;
        }
    }
}
