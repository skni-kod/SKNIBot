using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class EmojiCounterCommand : BaseCommandModule
    {
        [Command("policzEmoji")]
        [Description("Zlicza emoji użyte do tej pory na serwerze.")]
        public async Task CountEmoji(CommandContext ctx)
        {
            ctx.TriggerTypingAsync();
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == ctx.Guild.Id.ToString()).Include(p => p.Emojis).FirstOrDefault();
                if (dbServer != null)
                {

                    IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                    // List for emojis to print
                    List<EmojiHolder> emojiHolderList = new List<EmojiHolder>();
                    // Get server emoji
                    serverEmojiList = await ctx.Guild.GetEmojisAsync();

                    // Get emoji from database
                    List<Emoji> dbEmojis = dbServer.Emojis.OrderBy(p => p.UsageCount).ToList();

                    foreach (DiscordGuildEmoji emoji in serverEmojiList)
                    {
                        if (emoji.IsAnimated == true)
                        {
                            continue;
                        }

                        Emoji tempEmoji = dbEmojis.Where(p => p.EmojiID == emoji.Id.ToString()).FirstOrDefault();

                        if (tempEmoji != null)
                        {
                            EmojiHolder emojiHolder = new EmojiHolder(emoji.Name, emoji.Id, tempEmoji.UsageCount);
                            emojiHolderList.Add(emojiHolder);
                        }
                        else
                        {
                            EmojiHolder emojiHolder = new EmojiHolder(emoji.Name, emoji.Id, 0);
                            emojiHolderList.Add(emojiHolder);
                        }
                    }

                    string wholeMessage = "";
                    emojiHolderList = emojiHolderList.OrderByDescending(p => p.UsageCount).ToList();

                    foreach (EmojiHolder emoji in emojiHolderList)
                    {
                        wholeMessage += emoji.GetEmojiToSend() + "\n";
                        if (wholeMessage.Length > 1800)
                        {
                            await ctx.RespondAsync(wholeMessage);
                            wholeMessage = "";
                        }
                    }

                    if (wholeMessage != null)
                    {
                        await ctx.RespondAsync(wholeMessage);
                        return;
                    }

                }

                await ctx.RespondAsync("Na tym serwerze jeszcze nie użyto emoji.");
            }
        }
    }

    public class EmojiHolder
    {
        public string EmojiName { get; set; }
        public ulong EmojiID { get; set; }
        public int UsageCount { get; set; }

        public EmojiHolder(string name, ulong id, int count)
        {
            EmojiName = name;
            EmojiID = id;
            UsageCount = count;
        }

        public string GetEmojiToSend()
        {
            return "<:" + EmojiName + ":" + EmojiID + "> użyto: " + UsageCount;
        }
    }
}
