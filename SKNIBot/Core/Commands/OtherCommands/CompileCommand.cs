using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.CompilationContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Różne")]
    public class CompileCommand : BaseCommandModule
    {
        private const string ApiEndpoint = "https://api.jdoodle.com/v1/execute";
        private const char ArgNamePrefix = '%';

        private List<string> CodeArgAliases = new List<string> { "kod", "code" };
        private List<string> LangArgAliases = new List<string> { "język", "language", "lang" };
        private List<string> InputArgAliases = new List<string> { "input", "wejście" };
        private List<string> VersionArgAliases = new List<string> { "wersja", "version" };


        [Command("kompiluj")]
        [Description("Kompiluje dany kod źródłowy i wyświetla wynik. Lista języków pod adresem https://www.jdoodle.com/compiler-api/docs")]
        [Aliases("compile", "uruchom", "run")]
        public async Task Compile(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var inputMessage = ctx.RawArgumentString;

            // split message content by prefix and remove empty strings
            var inputSplited = inputMessage.Split(ArgNamePrefix).ToList();
            inputSplited.RemoveAll(s => s.Length == 0);

            string langName = "";
            string code = "";
            string input = "";
            string version = "0";

            // Parse message parts
            for (int i = 0; i < inputSplited.Count; i++)
            {
                var argName = inputSplited[i].Split(new[] { ' ', '\n' }).First().ToLower();

                if (LangArgAliases.Contains(argName))
                {
                    // parse language name
                    langName = inputSplited[i].Substring(argName.Length).Trim();
                }
                else if (CodeArgAliases.Contains(argName))
                {
                    // get code
                    code = inputSplited[i].Substring(argName.Length).Trim().Trim('`');
                }
                else if (InputArgAliases.Contains(argName))
                {
                    // get input list
                    input = inputSplited[i].Substring(argName.Length).Trim();
                }
                else if (VersionArgAliases.Contains(argName))
                {
                    // get version number
                    version = inputSplited[i].Substring(argName.Length).Trim();
                    int dummyResult;
                    if(int.TryParse(version, out dummyResult) == false)
                    {
                        // version isn't number
                        await ctx.RespondAsync("Version must be number");
                    }
                }
            }

            if (langName.Length == 0)
            {
                await ctx.RespondAsync("Langauge name is needed");
                return;
            }

            if (code.Length == 0)
            {
                await ctx.RespondAsync("Code is actually really needed");
                return;
            }

            var result = GetCompilationResult(langName, input, code, version);

            await ctx.RespondAsync(embed: GetEmbedResult(result));
        }

        private CompilationResultContainer GetCompilationResult(string language, string input, string code, string version)
        {
            var compileRequest = new CompileRequestContainer
            {
                clientId = SettingsLoader.Container.JDoodle_Client_ID,
                clientSecret = SettingsLoader.Container.JDoodle_Client_Secret,
                language = language,
                script = code,
                stdin = input,
                versionIndex = version
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
