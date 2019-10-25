using System;
using System.Collections.Generic;
using System.Text;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Helpers
{
    public static class DeveloperHelper
    {
        public static bool IsDeveloper(ulong discordID)
        {
            return SettingsLoader.DevelopersContainer.Developers.Contains(discordID.ToString());
        }
    }
}
