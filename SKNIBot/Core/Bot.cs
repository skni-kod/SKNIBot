using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net.WebSocket;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core
{
    public class Bot
    {
        private DiscordClient Client { get; set; }
        private CommandsNextModule Commands { get; set; }

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

            Client = new DiscordClient(connectionConfig);
            Client.SetWebSocketClient<WebSocket4NetClient>();

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefix = SettingsLoader.Container.Prefix,
                EnableDms = true,
                EnableMentionPrefix = true,
                CaseSensitive = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.SetHelpFormatter<CustomHelpFormatter>();
            Commands.CommandExecuted += Commands_CommandExecuted;
            Commands.CommandErrored += Commands_CommandErrored;

            await Client.ConnectAsync();
        }

        private void SetNetworkParameters()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                                                   SecurityProtocolType.Tls12;
        }

        private void RegisterCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var registerCommandsMethod = Commands.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "RegisterCommands" && p.IsGenericMethod);

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p.GetType() == typeof(CommandsGroupAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(Commands, null);
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
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "SKNI Bot",
                $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' " +
                $"but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            return Task.FromResult(0);
        }
    }
}
