using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class MessageStatsCommand
    {
        [Command("stats")]
        [Description("Wyświetla statystyki dotyczące aktualnego kanału.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Stats(CommandContext ctx)
        {
            var stats = new Dictionary<string, int>();

            var messagesList = new List<DiscordMessage>();
            var lastMessageID = ctx.Message.Id;

            while (true)
            {
                var last100Messages = await ctx.Channel.GetMessagesAsync(100, lastMessageID);
                messagesList.AddRange(last100Messages);

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageID = last100Messages.Last().Id;
            }

            foreach (var message in messagesList)
            {
                if (!stats.ContainsKey(message.Author.Username))
                {
                    stats[message.Author.Username] = 0;
                }

                stats[message.Author.Username]++;
            }

            var sortedResult = stats.OrderByDescending(p => p.Value).ToList();

            var response = new StringBuilder();
            response.Append($"**Statystyki dla kanału {ctx.Channel.Name}:**\n");

            foreach (var user in sortedResult)
            {
                var userName = user.Key;
                var userMessagesCount = user.Value;

                response.Append($"   {userName}: {userMessagesCount}\n");
            }

            await ctx.RespondAsync(response.ToString());
        }
    }
}
