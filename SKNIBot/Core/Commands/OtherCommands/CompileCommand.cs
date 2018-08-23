using System;
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
    public class CompileCommand : BaseCommandModule
    {
        private const string ApiEndpoint = "https://api.jdoodle.com/v1/execute";

        [Command("kompiluj")]
        [Description("Kompiluje dany kod źródłowy i wyświetla wynik. Składnia: !kompiluj [język], [wejście], [kod]. Lista języków pod adresem https://www.jdoodle.com/compiler-api/docs")]
        [Aliases("compile", "uruchom", "run")]
        public async Task Compile(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var splitInput = ctx.RawArgumentString.Split('\n').ToList();
            var header = splitInput[0].Split(',').ToList();

            var language = header[0].Trim();
            var input = header[1].Trim();

            var startIndex = splitInput[1].StartsWith("```", StringComparison.InvariantCulture) ? 2 : 1;
            var endIndex = splitInput[1].StartsWith("```", StringComparison.InvariantCulture) ? splitInput.Count - 1 : splitInput.Count;

            var code = "";
            for (var i = startIndex; i < endIndex; i++)
            {
                code += splitInput[i] + "\n";
            }

            code = code.Trim();

            try
            {
                var compilationResult = GetCompilationResult(language, input, code);
                var compilationEmbedResult = GetEmbedResult(compilationResult);

                await ctx.RespondAsync("", false, compilationEmbedResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
