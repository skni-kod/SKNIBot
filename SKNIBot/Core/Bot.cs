using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Net.WebSocket;
using SKNIBot.Core.Commands.YouTubeCommands;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core
{
    public class Bot
    {
        public static DiscordClient DiscordClient { get; set; }
        private CommandsNextExtension _commands { get; set; }

        public void Run()
        {
            Connect();
            SetNetworkParameters();
            RegisterCommands();
        }

        private async void Connect()
        {
            var connectionConfig = new DiscordConfiguration
            {
                Token = SettingsLoader.Container.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            DiscordClient = new DiscordClient(connectionConfig);
            //DiscordClient.SetWebSocketClient<WebSocket4NetCoreClient>();

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new [] {SettingsLoader.Container.Prefix},
                EnableDms = true,
                EnableMentionPrefix = true,
                CaseSensitive = false
            };

            _commands = DiscordClient.UseCommandsNext(commandsConfig);
            //_commands.SetHelpFormatter<CustomHelpFormatter>();
            _commands.CommandExecuted += Commands_CommandExecuted;
            _commands.CommandErrored += Commands_CommandErrored;

            await DiscordClient.ConnectAsync();
        }

        private void SetNetworkParameters()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void RegisterCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var registerCommandsMethod = _commands.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "RegisterCommands" && p.IsGenericMethod);

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p.GetType() == typeof(CommandsGroupAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(_commands, null);
                }
            }
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "SKNI Bot", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.FromResult(0);
        }

        private Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            var responseBuilder = new StringBuilder();

            switch (e.Exception)
            {
                case CommandNotFoundException _:
                {
                    responseBuilder.Append("Nieznana komenda, wpisz !help aby uzyskać listę wszystkich dostępnych.");
                    break;
                }

                case ChecksFailedException _:
                {
                    var failedCheck = ((ChecksFailedException)e.Exception).FailedChecks.First();
                    var permission = (RequirePermissionsAttribute)failedCheck;

                    responseBuilder.Append("Nie masz uprawnień do wykonania tej akcji :<\n");
                    responseBuilder.Append($"Wymagane: *{permission.Permissions.ToPermissionString()}*");
                    break;
                }

                case ArgumentException _:
                {
                    responseBuilder.Append($"Nieprawidłowe parametry komendy, wpisz `!help {e.Command.Name}` aby uzyskać ich listę.\n");
                    break;
                }

                default:
                {
                    responseBuilder.Append($"**{e.Exception.Message}**\n");
                    responseBuilder.Append($"{e.Exception.StackTrace}\n");
                    break;
                }
            }

            if (responseBuilder.Length != 0)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#CD171E")
                };
                embed.AddField("Błąd", responseBuilder.ToString());

                e.Context.RespondAsync("", false, embed);
            }

            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "SKNI Bot",
                $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' " +
                $"but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            return Task.FromResult(0);
        }
    }
}
