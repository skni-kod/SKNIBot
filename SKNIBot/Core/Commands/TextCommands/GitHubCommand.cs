using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class GitHubCommand
    {
        private Random _random;
        private const int MaxRepositoryID = 10000000;

        public GitHubCommand()
        {
            _random = new Random();
        }

        [Command("github")]
        [Description("Wyświetla link do losowego repozytorium na GitHubie.")]
        public async Task GitHub(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var repositoryID = _random.Next(MaxRepositoryID);
            var requestLink = $"https://api.github.com/repositories?since={repositoryID}";

            var webClient = new WebClient();
            webClient.Headers["User-Agent"] = "Bot";

            var response = webClient.DownloadString(requestLink);

            var responseContainer = JsonConvert.DeserializeObject<List<GitHubContainer>>(response);
            var repositoryLink = responseContainer[0].html_url;

            await ctx.RespondAsync(repositoryLink);
        }
    }
}
