using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Helpers
{
    public static class ExitCodes
    {
        public static readonly int settingsFileDoesntExits = 101;
        public static readonly int developerFileDoesntExist = 102;
        public static readonly int environmentVaraibleNotPresent = 103;
        public static readonly int environmentVariableHasWrongType = 104;
        public static readonly int otherErrorDuringReadingSettings = 105;
    }
}
