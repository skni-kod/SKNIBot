using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SKNIBot.Core.Services
{
    class EmojiCounterService
    {
        public async Task CountEmojiInMessage(DiscordMessage message)
        {

            if (message.Author.IsCurrent == true || message.Content == null)
            {
                // No content in message or bot is author
                return;
            }
            if (message.Content.Contains(":") == true)
            {
                using (var databaseContext = new DynamicDBContext())
                {
                    if (!databaseContext.Servers.Any(p => p.ServerID == message.Channel.Guild.Id.ToString()))
                    {
                        Server dbServer = new Server();
                        dbServer.ServerID = message.Channel.Guild.Id.ToString();
                        databaseContext.Servers.Add(dbServer);
                        databaseContext.SaveChanges();
                    }
                }

                IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                serverEmojiList = await message.Channel.Guild.GetEmojisAsync();

                if (serverEmojiList == null)
                {
                    return;
                }
                try
                {
                    foreach (DiscordGuildEmoji serverEmoji in serverEmojiList)
                    {
                        if (serverEmoji == null)
                        {
                            continue;
                        }
                        int emojiCount = 0;
                        string emojiName = ":" + serverEmoji.Name + ":";

                        if (message.Content.Contains(emojiName) == true)
                        {
                            emojiCount = Regex.Matches(message.Content, emojiName).Count;

                            using (var databaseContext = new DynamicDBContext())
                            {
                                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == message.Channel.Guild.Id.ToString()).FirstOrDefault();
                                if (!databaseContext.Emojis.Any(p => p.EmojiID == serverEmoji.Id.ToString()))
                                {
                                    Emoji dbEmoji = new Emoji();
                                    dbEmoji.EmojiID = serverEmoji.Id.ToString();
                                    dbEmoji.UsageCount = emojiCount;
                                    dbEmoji.ServerID = dbServer.ID;
                                    databaseContext.Emojis.Add(dbEmoji);
                                    databaseContext.SaveChanges();
                                }
                                else
                                {
                                    Emoji dbEmoji = databaseContext.Emojis.Where(p => p.EmojiID == serverEmoji.Id.ToString()).FirstOrDefault();
                                    dbEmoji.UsageCount += emojiCount;
                                    databaseContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in new message.");
                    Console.WriteLine("Message: " + message?.Content);
                    Console.WriteLine("Channel: " + message?.Channel);
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }
        }

        public async Task CountEmojiReaction(DiscordUser user, DiscordEmoji emoji, DiscordChannel channel)
        {
            if (user.IsCurrent == false)
            {
                using (var databaseContext = new DynamicDBContext())
                {
                    if (!databaseContext.Servers.Any(p => p.ServerID == channel.Guild.Id.ToString()))
                    {
                        Server dbServer = new Server();
                        dbServer.ServerID = channel.Guild.Id.ToString();
                        databaseContext.Servers.Add(dbServer);
                        databaseContext.SaveChanges();
                    }
                }

                IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                serverEmojiList = await channel.Guild.GetEmojisAsync();

                foreach (DiscordGuildEmoji serverEmoji in serverEmojiList)
                {
                    if (emoji == serverEmoji)
                    {
                        using (var databaseContext = new DynamicDBContext())
                        {
                            Server dbServer = databaseContext.Servers.Where(p => p.ServerID == channel.Guild.Id.ToString()).FirstOrDefault();
                            if (!databaseContext.Emojis.Any(p => p.EmojiID == serverEmoji.Id.ToString()))
                            {
                                Emoji dbEmoji = new Emoji();
                                dbEmoji.EmojiID = serverEmoji.Id.ToString();
                                dbEmoji.UsageCount = 1;
                                dbEmoji.ServerID = dbServer.ID;
                                databaseContext.Emojis.Add(dbEmoji);
                                databaseContext.SaveChanges();
                            }
                            else
                            {
                                Emoji dbEmoji = databaseContext.Emojis.Where(p => p.EmojiID == serverEmoji.Id.ToString()).FirstOrDefault();
                                dbEmoji.UsageCount += 1;
                                databaseContext.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
