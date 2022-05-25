using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using SKNIBot.Core.Database.Models.DynamicDB;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.UserMessageStatsService;
using SKNIBot.Core.Services.DateMessageStatsService;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class MessageStatsCommand : BaseCommandModule
    {
        private const int UsernameFieldLength = 40;
        private const int MessagesCountFieldLength = 20;

        private int TotalFieldsLength => UsernameFieldLength + MessagesCountFieldLength;

        public UserMessageStatsService _statsHookUser;
        public DateMessageStatsService _statsHookDate;

        public MessageStatsCommand(UserMessageStatsService userMessageStats)
        {
            _statsHookUser = userMessageStats;
        }

        [Command("updateStats")]
        [Description("Wymusza aktualizację cache statystyk wiadomości aktualnego kanału")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateStats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            var messages = await GetAllMessagesFromChannel(ctx.Channel);
            var userMessagesStats = CountUserMessages(messages);
            _statsHookUser.UpdateGroupedMessageCount(userMessagesStats, ctx.Channel.Id, ctx.Guild.Id);
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        
        [Command("stats")]
        [Description("Wyświetla statystyki dotyczące aktualnego kanału.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task Stats(CommandContext ctx)
        {
            await PostEmbedHelper.PostEmbed(ctx, "Stats",
                ":warning: Komenda w trakcie przebudowy. Tymczasowo odblokowana do testów");
            
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync("To będzie szybkie... :eyes:");

            var messageData = _statsHookUser.FetchGroupedMessageCount(ctx.Guild.Id, ctx.Channel.Id);
            var response = await GetStatsResponse(messageData, ctx.Channel.Name, ctx.Guild);
            
            await ctx.RespondAsync(response);
            
        }
        
        [Command("updateStatsAll")]
        [Description("Wymusza aktualizację cache statystyk wiadomości WSZYSTKICH KANAŁÓW. UŻYWAĆ JEDYNIE W OSTATECZNOŚCI!!!")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateStatsAll(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            foreach (var channel in ctx.Guild.Channels.Where(p => p.Value.Type == ChannelType.Text))
            {
                try
                {
                    var messages = await GetAllMessagesFromChannel(channel.Value);
                    var userMessagesStats = CountUserMessages(messages);
                    _statsHookUser.UpdateGroupedMessageCount(userMessagesStats, channel.Key, ctx.Guild.Id);
                }
                catch (UnauthorizedException e)
                {
                    
                }
            }
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        
        [Command("statsall")]
        [Description("Wyświetla statystyki dla wszystkich kanałów.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task StatsAll(CommandContext ctx)
        {
            await PostEmbedHelper.PostEmbed(ctx, "Stats",
                ":warning: Komenda w trakcie przebudowy. Tymczasowo odblokowana do testów");
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");

            var allMessages = new Dictionary<ulong, int>();

            foreach (var channel in ctx.Guild.Channels.Where(p => p.Value.Type == ChannelType.Text))
            {
                try
                {
                    var messages = _statsHookUser.FetchGroupedMessageCount(ctx.Guild.Id, channel.Key);
                    foreach (var stat in messages)
                    {
                        if (allMessages.ContainsKey(stat.Key))
                        {
                            allMessages[stat.Key] += stat.Value;
                        }
                        else
                        {
                            allMessages[stat.Key] = stat.Value;
                        }
                    }
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

            var sortedStats = allMessages.ToArray().OrderByDescending(p => p.Value);
            var response = await GetStatsResponse(sortedStats, "all", ctx.Guild);

            await ctx.RespondAsync(response);
        }
        
        [Command("updateMsgStats")]
        [Description("Wymusza aktualizację cache statystyk wiadomości aktualnego kanału")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateMsgStats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            var messages = await GetAllMessagesFromChannel(ctx.Channel);
            var userMessagesStats = GroupMessagesByMonths(messages);
            _statsHookDate.UpdateGroupedMessageCount(userMessagesStats, ctx.Channel.Id, ctx.Guild.Id);
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        
        [Command("msgstats")]
        [Description("Wyświetla statystyki wiadomości dla poszczególnych miesięcy.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStats(CommandContext ctx)
        {
            await PostEmbedHelper.PostEmbed(ctx, "Stats",
                ":warning: Komenda w trakcie przebudowy. Tymczasowo odblokowana do testów");
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync("To będzie szybkie... :eyes:");

            var messageData = _statsHookDate.FetchGroupedMessageCount(ctx.Guild.Id, ctx.Channel.Id);
            var response = await GetMsgStatsResponse(messageData, ctx.Channel.Name, ctx.Guild);
            
            await ctx.RespondAsync(response);
        }
        
        [Command("updateMsgStatsAll")]
        [Description("Wymusza aktualizację cache statystyk wiadomości WSZYSTKICH KANAŁÓW. UŻYWAĆ JEDYNIE W OSTATECZNOŚCI!!!")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateMsgStatsAll(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            foreach (var channel in ctx.Guild.Channels.Where(p => p.Value.Type == ChannelType.Text))
            {
                try
                {
                    var messages = await GetAllMessagesFromChannel(channel.Value);
                    var userMessagesStats = GroupMessagesByMonths(messages);
                    _statsHookDate.UpdateGroupedMessageCount(userMessagesStats, channel.Key, ctx.Guild.Id);

                }
                catch (UnauthorizedException e)
                {
                    
                }
            }
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        [Command("msgstatsall")]
        [Description("Wyświetla statystyki wiadomości we wszystkich kanałach dla poszczególnych miesięcy.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStatsAll(CommandContext ctx)
        {
            await PostEmbedHelper.PostEmbed(ctx, "Stats",
                ":warning: Komenda w trakcie przebudowy. Tymczasowo odblokowana do testów");
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");

            var allMessages = new Dictionary<string, int>();
            foreach (var channel in ctx.Guild.Channels.Where(p => p.Value.Type == ChannelType.Text))
            {
                try
                { 
                    var messageData = _statsHookDate.FetchGroupedMessageCount(ctx.Guild.Id, channel.Key);
                    foreach (var stat in messageData)
                    {
                        if (allMessages.ContainsKey(stat.Key))
                        {
                            allMessages[stat.Key] += stat.Value;
                        }
                        else
                        {
                            allMessages[stat.Key] = stat.Value;
                        }
                    }
                }
                catch (UnauthorizedException ex)
                {

                }
            }

            var groupedMessages = allMessages.ToArray().OrderByDescending(p => p.Value);
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
