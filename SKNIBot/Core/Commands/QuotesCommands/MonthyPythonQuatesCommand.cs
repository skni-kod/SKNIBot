using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.QuotesCommands
{
    //PÓŹNIEJ TO POPRAWIE, NIE BIJCIE :<
    [CommandsGroup("Cytaty")]
    class MonthyPythonQuatesCommand {
        string _grenade;

        public MonthyPythonQuatesCommand() {
            _grenade = @"*I Pan powiedział: Wpierw wyjąć musisz świętą Zawleczkę, potem masz zliczyć
                        do trzech, nie mniej, nie więcej. Trzy ma być liczbą, do której liczyć masz i
                        liczbą tą ma być trzy. Do czterech nie wolno ci liczyć, ani do dwóch.Masz
                        tylko policzyć do trzech. Pięć jest wykluczone. Gdy liczba trzy jako trzecia w
                        kolejności osiągnięta zostanie, wówczas rzucić masz Święty Granat Ręczny z
                        Antiochii w kierunku wroga, co naigrawał się z ciebie w polu widzenia twego, a
                        wówczas on kitę odwali. Amen.*";
        }

        [Command("HolyHandGrenade")]
        public async Task EightBall(CommandContext ctx) {
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync(_grenade);
        }
    }
}
