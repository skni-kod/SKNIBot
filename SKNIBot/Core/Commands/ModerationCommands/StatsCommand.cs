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
    public class MessageStatsCommand : BaseCommandModule
    {
        private const int UsernameFieldLength = 20;
        private const int MessagesCountFieldLength = 20;

        private int TotalFieldsLength => UsernameFieldLength + MessagesCountFieldLength;

        [Command("stats")]
        [Description("Wyświetla statystyki dotyczące aktualnego kanału.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Stats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var messages = await GetAllMessagesFromChannel(ctx);
            var userMessagesStats = CountUserMessages(messages);
            var response = GetResponse(userMessagesStats, ctx.Channel.Name);

            await ctx.RespondAsync(response);
        }

        private async Task<List<DiscordMessage>> GetAllMessagesFromChannel(CommandContext ctx)
        {
            var messagesList = new List<DiscordMessage>();
            var lastMessageID = ctx.Message.Id;

            while (true)
            {
                var last100Messages = await ctx.Channel.GetMessagesBeforeAsync(lastMessageID, 100);
                messagesList.AddRange(last100Messages);

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageID = last100Messages.Last().Id;
            }

            return messagesList;
        }

        private List<KeyValuePair<string, int>> CountUserMessages(List<DiscordMessage> messages)
        {
            var stats = new Dictionary<string, int>();

            foreach (var message in messages)
            {
                if (!stats.ContainsKey(message.Author.Username))
                {
                    stats[message.Author.Username] = 0;
                }

                stats[message.Author.Username]++;
            }

            return stats.OrderByDescending(p => p.Value).ToList();
        }

        private string GetResponse(List<KeyValuePair<string, int>> userMessagesStats, string channelName)
        {
            var response = new StringBuilder();

            response.Append($"Statystyki dla kanału **#{channelName}:**\n");
            response.Append("```");
            response.Append("Nazwa użytkownika".PadRight(UsernameFieldLength));
            response.Append("Liczba wiadomości".PadRight(MessagesCountFieldLength));
            response.Append("\n");
            response.Append(new string('-', TotalFieldsLength));
            response.Append("\n");

            foreach (var user in userMessagesStats)
            {
                var userName = user.Key.PadRight(UsernameFieldLength);
                var userMessagesCount = user.Value.ToString().PadRight(MessagesCountFieldLength);

                response.Append($"{userName}{userMessagesCount}\n");
            }

            response.Append("```");

            return response.ToString();
        }
    }
}
