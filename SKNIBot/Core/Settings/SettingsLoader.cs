using System.IO;
using Newtonsoft.Json;

namespace SKNIBot.Core.Settings
{
    public static class SettingsLoader
    {
        public static SettingsContainer Container { get; private set; }

        private static string _settingsFile = "settings.json";

        static SettingsLoader()
        {
            Load();
        }

        private static void Load()
        {
            using (var settingsFile = new StreamReader(_settingsFile))
            {
                Container = JsonConvert.DeserializeObject<SettingsContainer>(settingsFile.ReadToEnd());
            }
        }
    }
}
