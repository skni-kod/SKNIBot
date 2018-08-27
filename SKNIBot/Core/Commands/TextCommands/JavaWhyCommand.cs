using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using SKNIBot.Core.Database.Models;
using SKNIBot.Core.Extensions;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class JavaWhyCommand : BaseCommandModule
    {
        Random _random;

        public JavaWhyCommand()
        {
            _random = new Random();
        }

        [Command("JavaWhy")]
        [Description("Wszystkie znalezione dziwactwa Javy w jednym miejscu!")]
        public async Task JavaWhy(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var embed = new DiscordEmbedBuilder()
            {
                Color = ColorHelper.RandomColor()
            };

            //Find random java shenaningans in dynamic database
            using (var db = new DynamicDBContext())
            {
                var thingsList = db.JavaThings.ToList();
                var thing = thingsList.RandomItem();

                embed.AddField(thing.Title, thing.Content);
            }
            
            //find random image with JavaWhy category
            using (var db = new StaticDBContext())
            {
                var links = db.Media.Where(m => m.Command.Name == "JavaWhy" && m.Category.Name == "JavaWhy")
                    .Select(m => m.Link).ToList();

                embed.ImageUrl = links.RandomItem();
            }

            await ctx.RespondAsync(embed: embed);
        }

        [Command("JavaWhyAdd")]
        [Description("Dodaj dziwactwo")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task JavaWhyAdd(CommandContext ctx, string title, string content)
        {
            await ctx.TriggerTypingAsync();
            using (var db = new DynamicDBContext())
            {
                db.JavaThings.Add(new JavaThing()
                {
                    Title = title,
                    Content = content
                });
                
                await db.SaveChangesAsync();
                await ctx.RespondAsync("Dodano kolejne dziwactwo");
            }
        }
    }
}