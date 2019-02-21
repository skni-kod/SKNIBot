using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var response = await GetStatsResponse(userMessagesStats, ctx.Channel.Name, ctx.Guild);

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
            var response = await GetStatsResponse(userMessagesStats, "all", ctx.Guild);

            await ctx.RespondAsync(response);
        }

        [Command("msgstats")]
        [Description("Wyświetla statystyki wiadomości dla poszczególnych miesięcy.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            var messages = await GetAllMessagesFromChannel(ctx.Channel);
            var groupedMessages = GroupMessagesByMonths(messages);
            var response = await GetMsgStatsResponse(groupedMessages, ctx.Channel.Name, ctx.Guild);

            await ctx.RespondAsync(response);
        }

        [Command("msgstatsall")]
        [Description("Wyświetla statystyki wiadomości we wszystkich kanałach dla poszczególnych miesięcy.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStatsAll(CommandContext ctx)
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

                }
            }
            
            var groupedMessages = GroupMessagesByMonths(allMessages);
            var response = await GetMsgStatsResponse(groupedMessages, "all", ctx.Guild);

            await ctx.RespondAsync(response);
        }

        private async Task<List<DiscordMessage>> GetAllMessagesFromChannel(DiscordChannel channel)
        {
            var messagesList = new List<DiscordMessage>();
            var lastMessageID = (await channel.GetMessagesAsync(1)).FirstOrDefault()?.Id;

            while (lastMessageID != null)
            {
                var last100Messages = await channel.GetMessagesBeforeAsync(lastMessageID.Value, 100);
                messagesList.AddRange(last100Messages);

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageID = last100Messages.Last().Id;
            }

            return messagesList;
        }

        private IEnumerable<KeyValuePair<ulong, int>> CountUserMessages(List<DiscordMessage> messages)
        {
            return messages
                .GroupBy(p => p.Author.Id, (authorId, authorMessages) => new
                {
                    AuthorId = authorId,
                    MessagesCount = authorMessages.Count()
                })
                .Select(p => new KeyValuePair<ulong, int>(p.AuthorId, p.MessagesCount))
                .OrderByDescending(p => p.Value)
                .ToList();
        }

        private IEnumerable<KeyValuePair<string, int>> GroupMessagesByMonths(List<DiscordMessage> messages)
        {
            return messages
                .GroupBy(p => p.CreationTimestamp.ToString("yyyy/MM"), (monthAndYear, messagesInMonth) => new
                {
                    MonthAndYear = monthAndYear,
                    MessagesCount = messagesInMonth.Count()
                })
                .Select(p => new KeyValuePair<string, int>(p.MonthAndYear, p.MessagesCount))
                .OrderByDescending(p => p.Key)
                .ToList();
        }

        private async Task<string> GetStatsResponse(IEnumerable<KeyValuePair<ulong, int>> userMessagesStats, string channelName, DiscordGuild guild)
        {
            var response = new StringBuilder();

            response.Append($"Statystyki dla kanału **#{channelName}:**\n");
            response.Append("```");
            response.Append("Nazwa użytkownika".PadRight(UsernameFieldLength));
            response.Append("Liczba wiadomości".PadRight(MessagesCountFieldLength));
            response.Append("\n");
            response.Append(new string('-', TotalFieldsLength));
            response.Append("\n");

            foreach (var user in userMessagesStats.Take(25))
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

        private async Task<string> GetMsgStatsResponse(IEnumerable<KeyValuePair<string, int>> groupedMessages, string channelName, DiscordGuild guild)
        {
            var response = new StringBuilder();

            response.Append($"Statystyki dla kanału **#{channelName}:**\n");
            response.Append("```");
            response.Append("Data".PadRight(UsernameFieldLength));
            response.Append("Liczba wiadomości".PadRight(MessagesCountFieldLength));
            response.Append("\n");
            response.Append(new string('-', TotalFieldsLength));
            response.Append("\n");

            foreach (var monthData in groupedMessages.Take(25))
            {
                var month = monthData.Key.PadRight(UsernameFieldLength);
                var messagesCount = monthData.Value.ToString().PadRight(MessagesCountFieldLength);

                response.Append($"{month}{messagesCount}\n");
            }

            response.Append("```");

            return response.ToString();
        }
    }
}
