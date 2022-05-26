using System;
using System.Collections.Generic;
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
        private const int CacheReloadInterval = 1000 * 60 * 60 * 24;

        private int TotalFieldsLength => UsernameFieldLength + MessagesCountFieldLength;

        private UserMessageStatsService _statsHookUser;
        private DateMessageStatsService _statsHookDate;
        private Timer _cacheReloadTimer;

        public MessageStatsCommand(UserMessageStatsService userMessageStats, DateMessageStatsService dateMessageStats)
        {
            _statsHookUser = userMessageStats;
            _statsHookDate = dateMessageStats;
            _cacheReloadTimer = new Timer(CacheReloadCallback, null, 40000, Timeout.Infinite);
        }

        private async void CacheReloadCallback(object state)
        {
            _cacheReloadTimer.Change(CacheReloadInterval, Timeout.Infinite);
            Console.Out.WriteLine("Starting full stats cache reload... This will take a lot of time...");
            var servers = _statsHookUser.GetServerList();
            servers.UnionWith(_statsHookDate.GetServerList());
            foreach (var server in servers)
            {
                
                foreach (var channel in (await Bot.DiscordClient.GetGuildAsync(server)).Channels.Where(p => p.Value.Type == ChannelType.Text))
                {
                    try
                    {
                        var messageStats = await GetStatsOfChannel(channel.Value);
                        
                        _statsHookUser.UpdateGroupedMessageCount(messageStats.Item1, channel.Key, server);
                
                        _statsHookDate.UpdateGroupedMessageCount(messageStats.Item2, channel.Key, server);
                    }
                    catch (UnauthorizedException e)
                    {
                        _ = e;
                    }
                }
            }
            Console.Out.WriteLine("Stats cache reloaded!");
        }

        [Command("updateStats")]
        [Description("Wymusza aktualizację cache statystyk wiadomości aktualnego kanału")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateStats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... :eyes:");
            
            var messagesStats = await GetStatsOfChannel(ctx.Channel);
            _statsHookUser.UpdateGroupedMessageCount(messagesStats.Item1, ctx.Channel.Id, ctx.Guild.Id);
            
            _statsHookDate.UpdateGroupedMessageCount(messagesStats.Item2, ctx.Channel.Id, ctx.Guild.Id);
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        
        [Command("stats")]
        [Description("Wyświetla statystyki dotyczące aktualnego kanału.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task Stats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();


            var messageData = _statsHookUser.FetchGroupedMessageCount(ctx.Guild.Id, ctx.Channel.Id).OrderByDescending(p => p.Value);
            var response = await GetStatsResponse(messageData, ctx.Channel.Name, ctx.Guild);
            
            await ctx.RespondAsync(response);
            
        }
        
        [Command("updateStatsAll")]
        [Description("Wymusza aktualizację cache statystyk wiadomości WSZYSTKICH KANAŁÓW. UŻYWAĆ JEDYNIE W OSTATECZNOŚCI!!!")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task UpdateStatsAll(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To chwilę potrwa... Skocz sobie na obiad i do sklepu... :eyes:");
            
            foreach (var channel in ctx.Guild.Channels.Where(p => p.Value.Type == ChannelType.Text))
            {
                try
                {
                    var messageStats = await GetStatsOfChannel(channel.Value);
                    
                    _statsHookUser.UpdateGroupedMessageCount(messageStats.Item1, channel.Key, ctx.Guild.Id);
            
                    _statsHookDate.UpdateGroupedMessageCount(messageStats.Item2, channel.Key, ctx.Guild.Id);
                }
                catch (UnauthorizedException e)
                {
                    _ = e;
                }
            }
            
            await ctx.RespondAsync("Cache przeładowany! Nowe statystyki już dostępne");
        }
        
        [Command("statsall")]
        [Description("Wyświetla statystyki dla wszystkich kanałów.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task StatsAll(CommandContext ctx)
        {
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
                    _ = ex;
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
        
        /*
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
        */
        
        [Command("msgstats")]
        [Description("Wyświetla statystyki wiadomości dla poszczególnych miesięcy.")]
        //[RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var messageData = _statsHookDate.FetchGroupedMessageCount(ctx.Guild.Id, ctx.Channel.Id).OrderByDescending(p => p.Key);
            var response = GetMsgStatsResponse(messageData, ctx.Channel.Name);
            
            await ctx.RespondAsync(response);
        }
        
        /*
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
        */
        
        [Command("msgstatsall")]
        [Description("Wyświetla statystyki wiadomości we wszystkich kanałach dla poszczególnych miesięcy.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task MsgStatsAll(CommandContext ctx)
        {
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
                    _ = ex;
                }
            }

            var groupedMessages = allMessages.ToArray().OrderByDescending(p => p.Key);
            var response = GetMsgStatsResponse(groupedMessages, "all");

            await ctx.RespondAsync(response);
        }

        private async Task<List<DiscordMessage>> GetAllMessagesFromChannel(DiscordChannel channel)
        {
            var messagesList = new List<DiscordMessage>();
            var lastMessageId = (await channel.GetMessagesAsync(1)).FirstOrDefault()?.Id;

            while (lastMessageId != null)
            {
                var last100Messages = await channel.GetMessagesBeforeAsync(lastMessageId.Value, 100);
                messagesList.AddRange(last100Messages);

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageId = last100Messages.Last().Id;
            }

            return messagesList;
        }

        private async Task<(IEnumerable<KeyValuePair<ulong, int>>, IEnumerable<KeyValuePair<string, int>>)> GetStatsOfChannel(DiscordChannel channel)
        {
            var messagesListByUser = new Dictionary<ulong, int>();
            var messagesListByDate = new Dictionary<string, int>();
            var lastMessageId = (await channel.GetMessagesAsync(1)).FirstOrDefault()?.Id;

            while (lastMessageId != null)
            {
                var last100Messages = await channel.GetMessagesBeforeAsync(lastMessageId.Value, 100);
                var lastUserGroupedMessages = last100Messages
                    .GroupBy(p => p.Author.Id, (authorId, messages) =>
                        new
                        {
                            AuthorId = authorId,
                            MessagesCount = messages.Count()
                        });
                var lastDateGroupedMessages = last100Messages
                    .GroupBy(p => p.CreationTimestamp.ToString("yyyy/MM"), (monthAndYear, messagesInMonth) => new
                    {
                        Date = monthAndYear,
                        MessagesCount = messagesInMonth.Count()
                    });

                foreach (var message in lastUserGroupedMessages)
                {
                    if (messagesListByUser.ContainsKey(message.AuthorId))
                    {
                        messagesListByUser[message.AuthorId] += message.MessagesCount;
                    }
                    else
                    {
                        messagesListByUser[message.AuthorId] = message.MessagesCount;
                    }
                }

                foreach (var message in lastDateGroupedMessages)
                {
                    if (messagesListByDate.ContainsKey(message.Date))
                    {
                        messagesListByDate[message.Date] += message.MessagesCount;
                    }
                    else
                    {
                        messagesListByDate[message.Date] = message.MessagesCount;
                    }
                }

                if (last100Messages.Count < 100)
                {
                    break;
                }

                lastMessageId = last100Messages.Last().Id;
            }

            return (messagesListByUser.AsEnumerable(), messagesListByDate.AsEnumerable());
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
                    _ = ex;
                }
            }

            response.Append("```");

            return response.ToString();
        }

        private string GetMsgStatsResponse(IEnumerable<KeyValuePair<string, int>> groupedMessages, string channelName)
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
                var month = monthData.Key.PadRight(7, '0').PadRight(UsernameFieldLength);
                var messagesCount = monthData.Value.ToString().PadRight(MessagesCountFieldLength);

                response.Append($"{month}{messagesCount}\n");
            }

            response.Append("```");

            return response.ToString();
        }
    }
}
