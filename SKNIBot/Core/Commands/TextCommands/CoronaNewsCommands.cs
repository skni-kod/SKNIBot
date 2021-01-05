using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class CoronaNewsCommands : BaseCommandModule
    {
        private List<List<string>> messages = new List<List<string>>
        {
            new List<string>{"Ale numer...", "Potwierdzone info!", "Słów nie ma co oni wyprawiają!", "Puść dalej!",
                             "Szok, nie mogę w to uwierzyć...", "A to już słyszałeś?"},
            new List<string>{"Wiarygodne źródło,", "Moja koleżanka,", "Mój kuzyn,", "Syn sąsiada,",
                             "Brat naszego proboszcza,", "Kumpel mojego szefa,"},
            new List<string>{"z Instytutu Chorób Zakaźnych", "pracuje w strukturach rządowych i", "oficer ABW,", "lekarz-epidemiolog",
                             "dzienikarz w TVP,", "szkolny kolega Agaty Dudy, no wiesz prezydentowej,"},
            new List<string>{"dał(a) cynk, że prezydent", "właśnie pisze mi w mailu, że rząd", "wie na 100%, że sejm", "przysłał(a) mi SMS-a, że ministerstwo zdrowia",
                             "właśnie dostał(a) komunikat, że sztab WP", "przypakdiem dowiedział(a) się, że premier"},
            new List<string>{"jutro w nocy", "w ciągu 24 godzin", "za chwilę", "w najbliższych godzinach",
                             "w najbliższą sobotę", "na dniach"},
            new List<string>{"wyśle wszystkich 60+ do ośrodków izolacyjnych.", "zamknie wjazd do Warszawy.", "nakarze zamknięcie sklepów.", "wprowadzi wojsko na ulice.",
                             "ma zakazać używania gotówki.", "zablokuje konta bankowe osób na litery od A do L."}
        };
        [Command("koronneWiadomości")]
        [Aliases("koronneWiadomosci","koronnyNews")]
        [Description("Wyświetl zmyslone wiadomości o koronawirusie.")]
        public async Task ShowNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            Random rnd = new Random();
            string msg = "";
            for (int i = 0; i < messages.Count; i++)
            {
                int index = rnd.Next(0, messages[i].Count);
                msg += messages[i][index];
                msg += " ";
            }
            await ctx.RespondAsync(msg);
        }
    }
}
