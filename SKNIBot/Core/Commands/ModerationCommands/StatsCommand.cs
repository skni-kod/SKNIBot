using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class MessageStatsCommand : BaseCommandModule
    {
        private const int UsernameFieldLength = 40;
        private const int MessagesCountFieldLength = 20;

        private int TotalFieldsLength => UsernameFieldLength + MessagesCountFieldLength;

        [Command("stats")]
        [Description("Wyświetla statystyki dotyczące aktualnego kanału.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Stats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync("To chwilę potrwa... :eyes:");

            var messages = await GetAllMessagesFromChannel(ctx.Channel);
            var userMessagesStats = CountUserMessages(messages);
            var response = await GetResponse(userMessagesStats, ctx.Channel.Name, ctx.Guild);

            await ctx.RespondAsync(response);
        }

        [Command("statsall")]
        [Description("Wyświetla statystyki dla wszystkich kanałów.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task StatsAll(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");

            var allMessages = new List<DiscordMessage>();

            foreach (var channel in ctx.Guild.Channels.Where(p => p.Type == ChannelType.Text))
            {
                try
                {
                    var messages = await GetAllMessagesFromChannel(channel);
                    allMessages.AddRange(messages);
                }
                catch (UnauthorizedException ex)
                {
                  /*        Brak dostępu
                    __________████████_____██████
                    _________█░░░░░░░░██_██░░░░░░█
                    ________█░░░░░░░░░░░█░░░░░░░░░█
                    _______█░░░░░░░███░░░█░░░░░░░░░█
                    _______█░░░░███░░░███░█░░░████░█
                    ______█░░░██░░░░░░░░███░██░░░░██
                    _____█░░░░░░░░░░░░░░░░░█░░░░░░░░███
                    ____█░░░░░░░░░░░░░██████░░░░░████░░█
                    ____█░░░░░░░░░█████░░░████░░██░░██░░█
                    ___██░░░░░░░███░░░░░░░░░░█░░░░░░░░███
                    __█░░░░░░░░░░░░░░█████████░░█████████
                    _█░░░░░░░░░░█████_████___████_█████___█
                    _█░░░░░░░░░░█______█_███__█_____███_█___█
                     █░░░░░░░░░░░░█___████_████____██_██████
                      ░░░░░░░░░░░░░█████████░░░████████░░░█
                      ░░░░░░░░░░░░░░░░█░░░░░█░░░░░░░░░░░░█
                      ░░░░░░░░░░░░░░░░░░░░██░░░░█░░░░░░██
                      ░░░░░░░░░░░░░░░░░░██░░░░░░░███████
                      ░░░░░░░░░░░░░░░░██░░░░░░░░░░█░░░░░█
                      ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                      ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                      ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                      ░░░░░░░░░░░█████████░░░░░░░░░░░░░░██
                      ░░░░░░░░░░█▒▒▒▒▒▒▒▒███████████████▒▒█
                      ░░░░░░░░░█▒▒███████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█
                      ░░░░░░░░░█▒▒▒▒▒▒▒▒▒█████████████████
                      ░░░░░░░░░░████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█
                      ░░░░░░░░░░░░░░░░░░██████████████████
                      ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                      ██░░░░░░░░░░░░░░░░░░░░░░░░░░░██
                      ▓██░░░░░░░░░░░░░░░░░░░░░░░░██
                      ▓▓▓███░░░░░░░░░░░░░░░░░░░░█
                      ▓▓▓▓▓▓███░░░░░░░░░░░░░░░██
                      ▓▓▓▓▓▓▓▓▓███████████████▓▓█
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█
                   */
                }
            }

            var userMessagesStats = CountUserMessages(allMessages);
            var response = await GetResponse(userMessagesStats, "all", ctx.Guild);

            await ctx.RespondAsync(response);
        }

        private async Task<List<DiscordMessage>> GetAllMessagesFromChannel(DiscordChannel channel)
        {
            var messagesList = new List<DiscordMessage>();
            var lastMessageID = (await channel.GetMessagesAsync(1))[0].Id;

            while (true)
            {
                var last100Messages = await channel.GetMessagesBeforeAsync(lastMessageID, 100);
                messagesList.AddRange(last100Messages);

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageID = last100Messages.Last().Id;
            }

            return messagesList;
        }

        private List<KeyValuePair<ulong, int>> CountUserMessages(List<DiscordMessage> messages)
        {
            var stats = new Dictionary<ulong, int>();

            foreach (var message in messages)
            {
                if (!stats.ContainsKey(message.Author.Id))
                {
                    stats[message.Author.Id] = 0;
                }

                stats[message.Author.Id]++;
            }

            return stats.OrderByDescending(p => p.Value).ToList();
        }

        private async Task<string> GetResponse(List<KeyValuePair<ulong, int>> userMessagesStats, string channelName, DiscordGuild guild)
        {
            var response = new StringBuilder();

            response.Append($"Statystyki dla kanału **#{channelName}:**\n");
            response.Append("```");
            response.Append("Nazwa użytkownika".PadRight(UsernameFieldLength));
            response.Append("Liczba wiadomości".PadRight(MessagesCountFieldLength));
            response.Append("\n");
            response.Append(new string('-', TotalFieldsLength));
            response.Append("\n");

            foreach (var user in userMessagesStats.Where(p => p.Value >= 10))
            {
                try
                {
                    var displayName = (await guild.GetMemberAsync(user.Key)).DisplayName;
                    var userName = displayName.PadRight(UsernameFieldLength);
                    var userMessagesCount = user.Value.ToString().PadRight(MessagesCountFieldLength);

                    response.Append($"{userName}{userMessagesCount}\n");
                }
                catch (NotFoundException ex)
                {

                }
            }

            response.Append("```");

            return response.ToString();
        }
    }
}
