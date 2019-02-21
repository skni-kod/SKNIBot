using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Helpers
{
    public static class ColorHelper
    {
        public static DiscordColor RandomColor()
        {
            Random rand = new Random();
            byte r = (byte)rand.Next(0, 255);
            byte g = (byte)rand.Next(0, 255);
            byte b = (byte)rand.Next(0, 255);

            return new DiscordColor(r, g, b);
        }
    }
}
