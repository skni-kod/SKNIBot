using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    public static class EmotionalTitanicFluteRespond
    {
        private static bool _emotionalTitanicFluteHasBeenSentToday;
        private static int _dayWhenEmotionalTitanicFluteHasBeenSent;

        static EmotionalTitanicFluteRespond()
        {
            _emotionalTitanicFluteHasBeenSentToday = true;
            _dayWhenEmotionalTitanicFluteHasBeenSent = DateTime.Now.Day;
        }

        [MessageRespond]
        public static async Task Respond(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            if (DateTime.Now.Day != _dayWhenEmotionalTitanicFluteHasBeenSent)
            {
                _emotionalTitanicFluteHasBeenSentToday = false;
                _dayWhenEmotionalTitanicFluteHasBeenSent = DateTime.Now.Day;
            }

            if (!_emotionalTitanicFluteHasBeenSentToday)
            {
                // Absolutely emotional Titanic flute
                await args.Channel.SendMessageAsync("https://www.youtube.com/watch?v=KolfEhV-KiA");

                _emotionalTitanicFluteHasBeenSentToday = true;
            }
        }
    }
}
