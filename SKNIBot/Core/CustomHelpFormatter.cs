using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace SKNIBot.Core
{
    public class CustomHelpFormatter : IHelpFormatter
    {
        private string _commandName;
        private string _commandDescription;
        private List<string> _aliases;
        private List<string> _parameters;
        private Dictionary<string, List<string>> _subCommands;

        private const string Color = "#5588EE";

        public CustomHelpFormatter()
        {
            _aliases = new List<string>();
            _parameters = new List<string>();
            _subCommands = new Dictionary<string, List<string>>();
        }

        public IHelpFormatter WithCommandName(string name)
        {
            _commandName = name;
            return this;
        }

        public IHelpFormatter WithDescription(string description)
        {
            _commandDescription = description;
            return this;
        }

        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            foreach (var argument in arguments)
            {
                var argumentBuilder = new StringBuilder();
                argumentBuilder.Append($"`{argument.Name}`: {argument.Description}");

                if (argument.DefaultValue != null)
                {
                    argumentBuilder.Append($" Default value: {argument.DefaultValue}");
                }

                _parameters.Add(argumentBuilder.ToString());
            }

            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            foreach (var alias in aliases)
            {
                _aliases.Add($"`{alias}`");
            }

            return this;
        }

        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p is CommandsGroupAttribute))
                {
                    var groupAttribute = (CommandsGroupAttribute)attributes.First(p => p is CommandsGroupAttribute);
                    var commandHandlers = type.GetMethods();

                    foreach (var method in commandHandlers)
                    {
                        var methodAttributes = method.GetCustomAttributes();
                        if (methodAttributes.Any(p => p is CommandAttribute))
                        {
                            if (!_subCommands.ContainsKey(groupAttribute.Group))
                            {
                                _subCommands.Add(groupAttribute.Group, new List<string>());
                            }

                            _subCommands[groupAttribute.Group].Add($"`{method.Name}`");
                        }
                    }
                }
            }

            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            return this;
        }

        public CommandHelpMessage Build()
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Color)
            };

            return _commandName == null ? BuildGeneralHelp(embed) : BuildCommandHelp(embed);
        }

        private CommandHelpMessage BuildGeneralHelp(DiscordEmbedBuilder embed)
        {
            embed.AddField("HELP", "Wpisz !help <command_name> aby uzyskać więcej informacji.");

            foreach (var group in _subCommands)
            {
                embed.AddField(group.Key, string.Join(", ", group.Value));
            }

            return new CommandHelpMessage(String.Empty, embed);
        }

        private CommandHelpMessage BuildCommandHelp(DiscordEmbedBuilder embed)
        {
            embed.AddField(_commandName, _commandDescription);

            if(_aliases.Count > 0) embed.AddField("Aliasy", string.Join(", ", _aliases));
            if(_parameters.Count > 0) embed.AddField("Parametry", string.Join("\r\n", _parameters));

            return new CommandHelpMessage(String.Empty, embed);
        }
    }
}
