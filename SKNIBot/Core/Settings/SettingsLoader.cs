using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Settings
{
    [Serializable]
    public class VariableNotPresentException : Exception
    {
        public VariableNotPresentException()
        { }

        public VariableNotPresentException(string message)
            : base(message)
        { }

        public VariableNotPresentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    [Serializable]
    public class VariableHasWrongTypeException : Exception
    {
        public VariableHasWrongTypeException()
        { }

        public VariableHasWrongTypeException(string message)
            : base(message)
        { }

        public VariableHasWrongTypeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    /// <summary>
    /// Settings loader class that provides functionality to load settings for application.
    /// </summary>
    public static class SettingsLoader
    {
        public static SettingsContainer SettingsContainer { get; private set; }
        public static DevelopersContainer DevelopersContainer { get; private set; }

#if DEBUG
        private static readonly string _settingsFile = "debug.json";
#else
        private static readonly string _settingsFile = "release.json";
#endif
        private static readonly string _developersFile = "developers.json";

#if DEBUG
        private static readonly string _skniBotUseEnv = "SKNIBotDebugUseEnv";
        private static readonly string _skniBotDiscordToken = "SKNIBotDebugDiscordToken";
        private static readonly string _skniBotDiscordPrefix = "SKNIBotDebugDiscordPrefix";
        private static readonly string _skniBotClubServer = "SKNIBotDebugClubServer";
        private static readonly string _skniBotWaaaiApiKey = "SKNIBotDebugWaaaiApiKey";
        private static readonly string _skniBotJDoodleClientId = "SKNIBotDebugJDoodleClientId";
        private static readonly string _skniBotJDoodleClientSecret = "SKNIBotDebugJDoodleClientSecret";
        private static readonly string _skniBotSpotifyClientId = "SKNIBotDebugSpotifyClientId";
        private static readonly string _skniBotSpotifyClientSecret = "SKNIBotDebugSpotifyClientSecret";
        private static readonly string _skniBotDevelopers = "SKNIBotDebugDevelopers";
#else
        private static readonly string _skniBotUseEnv = "SKNIBotUseEnv";
        private static readonly string _skniBotDiscordToken = "SKNIBotDiscordToken";
        private static readonly string _skniBotDiscordPrefix = "SKNIBotDiscordPrefix";
        private static readonly string _skniBotClubServer = "SKNIBotClubServer";
        private static readonly string _skniBotWaaaiApiKey = "SKNIBotWaaaiApiKey";
        private static readonly string _skniBotJDoodleClientId = "SKNIBotJDoodleClientId";
        private static readonly string _skniBotJDoodleClientSecret = "SKNIBotJDoodleClientSecret";
        private static readonly string _skniBotSpotifyClientId = "SKNIBotSpotifyClientId";
        private static readonly string _skniBotSpotifyClientSecret = "SKNIBotSpotifyClientSecret";
        private static readonly string _skniBotDevelopers = "SKNIBotDevelopers";
#endif

        private static EnvironmentVariableTarget _target;

        /// <summary>
        /// Default constructor that loads settings and developers for application.
        /// </summary>
        static SettingsLoader()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _target = EnvironmentVariableTarget.Machine;
            }
            else
            {
                _target = EnvironmentVariableTarget.Process;
            }

            string useEnv = System.Environment.GetEnvironmentVariable(_skniBotUseEnv, _target);
            if(useEnv != null && useEnv == "true")
            {
                LoadFromEnvorinment();
            }
            else
            {
                LoadFromFile();
            }
        }

        /// <summary>
        /// Loads settings and developersfrom environoment variables.
        /// </summary>
        private static void LoadFromEnvorinment()
        {
            try
            {
                SettingsContainer = new SettingsContainer
                {
                    Token = GetStringFromEnvironment(_skniBotDiscordToken),
                    Prefix = GetStringFromEnvironment(_skniBotDiscordPrefix),
                    ClubServer = GetUlongFromEnvironment(_skniBotClubServer),
                    Waaai_Key = GetStringFromEnvironment(_skniBotWaaaiApiKey),
                    JDoodle_Client_ID = GetStringFromEnvironment(_skniBotJDoodleClientId),
                    JDoodle_Client_Secret = GetStringFromEnvironment(_skniBotJDoodleClientSecret),
                    Spotify_Client_Id = GetStringFromEnvironment(_skniBotSpotifyClientId),
                    Spotify_Client_Secret = GetStringFromEnvironment(_skniBotSpotifyClientSecret)
                };
                DevelopersContainer = new DevelopersContainer
                {
                    Developers = new List<string>(GetStringFromEnvironment(_skniBotDevelopers).Split(';'))
                };
            }
            catch(VariableNotPresentException eq)
            {
                Console.WriteLine(eq.Message);
                System.Environment.Exit(ExitCodes.environmentVaraibleNotPresent);
            }
            catch(VariableHasWrongTypeException eq)
            {
                Console.WriteLine(eq.Message);
                System.Environment.Exit(ExitCodes.environmentVariableHasWrongType);
            }
        }

        /// <summary>
        /// Loads settings and developers from file.
        /// </summary>
        private static void LoadFromFile()
        {
            try
            {
                var settingsFile = new StreamReader(_settingsFile);
                SettingsContainer = JsonConvert.DeserializeObject<SettingsContainer>(settingsFile.ReadToEnd());
                settingsFile.Close();
            }
            catch(FileNotFoundException eq)
            {
                Console.WriteLine(eq.Message);
                System.Environment.Exit(ExitCodes.settingsFileDoesntExits);
            }

            try
            {
                var developersFile = new StreamReader(_developersFile);
                DevelopersContainer = JsonConvert.DeserializeObject<DevelopersContainer>(developersFile.ReadToEnd());
                developersFile.Close();
            }
            catch (FileNotFoundException eq)
            {
                Console.WriteLine(eq.Message);
                System.Environment.Exit(ExitCodes.developerFileDoesntExist);
            }

        }

        /// <summary>
        /// Loads string from environment variable.
        /// </summary>
        /// <param name="variable">Environment varaible name.</param>
        /// <returns>>Value of environment varaible as string.</returns>
        private static string GetStringFromEnvironment(string variable)
        {
            string envVariable = System.Environment.GetEnvironmentVariable(variable, _target);
            if(envVariable == null)
            {
                throw new VariableNotPresentException("Variable " + variable + " not present in environemnt");
            }
            else
            {
                return envVariable;
            }
        }

        /// <summary>
        /// Loads ulong from environment variable.
        /// </summary>
        /// <param name="variable">Environment varaible name.</param>
        /// <returns>Value of environment varaible as ulong.</returns>
        private static ulong GetUlongFromEnvironment(string variable)
        {
            string envVariable = System.Environment.GetEnvironmentVariable(variable, _target);
            if (envVariable == null)
            {
                throw new VariableNotPresentException("Variable " + variable + " not present in environemnt");
            }
            if (ulong.TryParse(envVariable, out ulong result))
            {
                return result;
            }
            else
            {
                throw new VariableHasWrongTypeException("Variable " + variable + " is not correct type. Should be ulong, but is " + envVariable);
            }
        }
    }
}
