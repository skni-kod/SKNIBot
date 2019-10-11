using System.IO;
using Newtonsoft.Json;

namespace SKNIBot.Core.Settings
{
    public static class SettingsLoader
    {
        public static SettingsContainer SettingsContainer { get; private set; }
        public static DevelopersContainer DevelopersContainer { get; private set; }

        private static string _settingsFile = "";
        private static string _developersFile = "developers.json";

        static SettingsLoader()
        {
#if DEBUG
            _settingsFile = "debug.json";
#else
            _settingsFile = "release.json";
#endif
            Load();
        }

        private static void Load()
        {
            using (var settingsFile = new StreamReader(_settingsFile))
            {
                SettingsContainer = JsonConvert.DeserializeObject<SettingsContainer>(settingsFile.ReadToEnd());
            }
            using (var developersFile = new StreamReader(_developersFile))
            {
                DevelopersContainer = JsonConvert.DeserializeObject<DevelopersContainer>(developersFile.ReadToEnd());
            }
        }
    }
}
