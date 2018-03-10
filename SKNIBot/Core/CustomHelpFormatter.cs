using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace SKNIBot.Core
{
    public class CustomHelpFormatter : IHelpFormatter
    {
        private DiscordEmbedBuilder embed;

        private const string Color = "#FF0000";
        private const string Title = "HELP";

        public CustomHelpFormatter()
        {
            embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Color)
            };

            embed.AddField(Title, "Wpisz !help <command_name> aby uzyskać więcej informacji.\r\n");
        }

        public IHelpFormatter WithCommandName(string name)
        {
            return this;
        }

        public IHelpFormatter WithDescription(string description)
        {
            return this;
        }

        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            return this;
        }

        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            embed.AddField("Commands", string.Join(", ", subcommands.Select(p => p.Name)));
            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            return this;
        }

        public CommandHelpMessage Build()
        {
            return new CommandHelpMessage(string.Empty, embed);
        }
    }
}
