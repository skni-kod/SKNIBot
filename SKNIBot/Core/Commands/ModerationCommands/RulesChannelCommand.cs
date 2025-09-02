using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RulesChannelCommand : BaseCommandModule
    {
        [Command("powitanie")]
        [Description("Wyświetla powitalną wiadomość dla nowych członków z dynamicznymi rolami.")]
        public async Task ShowCustomWelcome(CommandContext ctx)
        {
            ulong prezesRoleId = 381442719731679244;
            ulong gameDevRoleId = 500066785693794304;
            ulong elektronikaRoleId = 500066692936892437;
            ulong aplikacjeWebRoleId = 500066513622007849;
            ulong skarbnikRoleId = 903633200130183248;
            
            string GetUsersWithRole(ulong roleId)
            {
                var role = ctx.Guild.Roles.Values.FirstOrDefault(r => r.Id == roleId);
                if (role == null)
                    return "[Rola nie znaleziona]";
                
                var users = ctx.Guild.Members.Values
                    .Where(m => m.Roles.Contains(role))
                    .Select(m => m.DisplayName)
                    .ToList();

                if (users.Count == 0)
                    return "[Puste]";
                if (users.Count == 1)
                    return users[0];
                return string.Join(", ", users);
            }

            // Budowanie wiadomości z dynamicznymi rolami
            var message1 = $@"
Dzięki za dołączenie do Discorda SKNI KOD!

Nasze koło podzielone jest na 3 sekcje
🕹️ game dev
📺 elektroniki i retro
🖥️ aplikacji webowych, desktopowych i mobilnych

👨‍💼 Prezes:
{GetUsersWithRole(prezesRoleId)}

👨‍💻 Zarząd:
Skarbnik: {GetUsersWithRole(skarbnikRoleId)}
Opiekun sekcji elektroniki i retro: {GetUsersWithRole(elektronikaRoleId)}
Opiekun sekcji game dev: {GetUsersWithRole(gameDevRoleId)}
Opiekun sekcji aplikacji webowych, desktopowych i mobilnych: {GetUsersWithRole(aplikacjeWebRoleId)}

Ważne linki:
🌐 Strona koła: https://kod.prz.edu.pl/
🌐 Fanpage Koła: http://fb.com/skni.kod";
var message2 = $@"
Regulamin:
• Nieznajomość regulaminu nie zwalnia z jego przestrzegania.
• Zarząd Koła i Core, później określane jako administracja, ma pełne prawa do zmiany regulaminu. Informacje o zmianie regulaminu będą ogłaszane na odpowiednim kanale.
• Administracja może upomnieć lub wyrzucić członka w sytuacjach niewymienionych w regulaminie, jeśli uzna że naruszają standardy społeczności.
• Regulamin jest tylko zbiorem wytycznych, ale ostateczną decyzję podejmuje administracja.
• Zakaz łamania regulaminu discorda. https://discord.com/terms
• Na serwerze stosujemy następujący schemat nazw użytkowników - Imię “Ksywa” Nazwisko.
• Jako że discord jest głównym kanałem komunikacji w kole, konieczne jest podanie prawdziwego imienia i nazwiska.
• Prosimy o nie wyciszanie kategorii tablica, na której znajdują się kanały z regulaminem i ogłoszeniami.
• Trzymamy się tematyki kanałów (zwłaszcza projektowych) na których piszemy.
• Udostępnienie jakichkolwiek łącz do stron sugerujących otrzymanie darmowego nitro lub mających znamiona masowego spamu (brak opisu, łamana polszczyzna, charakterystyczna domena, itp.), będzie skutkowało usunięciem tej wiadomości oraz wyrzuceniem konta z serwera
• Nie bądź niemiły i szanuj innych. (prywatne spięcia rozwiązujcie na pw).
• Zakaz udostępnia treści +18/NSFW. To serwer organizacji studenckiej więc nie wypada, żeby takie treści się tu znajdowały.
• Zakaz promowania rasizmu, homofobii i podobnych zachowań. 
• Zakaz spamu.
• Zakaz masowego pingowania i postowania treści reklamowych bez zgody administracji.
• Zakaz udostępniania cudzych danych osobowych bez pozwolenia tejże osoby.
• Zakaz promowania i udostępniania pirackich programów i treści.
• Wrzucamy kod w bloki kodu. Krótki poradnik:
(opcjonalnie język, np: c, py, python)
    TUTAJ_KOD 

 Znaczek `, tzw. backtick jest na tym samym klawiszu co ~ -> tuż pod ESC

• Jeśli macie jakieś pytania odnośnie regulaminu zadawajcie je administracji.
• Just don’t be a dick.
";

            await ctx.RespondAsync(message1);
            await ctx.RespondAsync(message2);
        }
    }
}
