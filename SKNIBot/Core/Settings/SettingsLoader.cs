using System.IO;
using Newtonsoft.Json;

namespace SKNIBot.Core.Settings
{
    public static class SettingsLoader
    {
        public static SettingsContainer Container { get; private set; }

        private static string _settingsFile = "";

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
                Container = JsonConvert.DeserializeObject<SettingsContainer>(settingsFile.ReadToEnd());
            }
        }
    }
}
