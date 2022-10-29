using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.GamesConst;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand : BaseCommandModule
    {
        private const int Stages = 9;

        private Random _random;
        /// <summary>
        /// Wszystkie aktualne gry
        /// </summary>
        private List<HangmanGame> HangmanGames;
        /// <summary>
        /// Obecnie załadowana gra
        /// </summary>
        private HangmanGame CurrentGame;

        public HangmanCommand()
        {
            _random = new Random();
            HangmanGames = new List<HangmanGame>();
        }

        [Command("wisielec")]
        [Description("Gra w wisielca. Możliwe kategorie `państwa`, `zwierzęta`, `rzeczy` lub brak kategrii.")]
        [Aliases("hangman", "w", "h")]
        public async Task Hangman(CommandContext ctx, [Description("Kategoria")] string type = null)
        {
            await ctx.TriggerTypingAsync();
            LoadGame(ctx.Message.ChannelId);
            var output = "";
            //Jeżeli gra nie jest rozpoczęta, rozpocznij
            if (CurrentGame.gameStarted == false)
            {
                StartGame(type);
            }
            //Gdy brak litery
            else
            {
                if (type == null)
                {
                    output += "Podaj literę \n";
                }
                else if (type == CurrentGame.word)
                {
                    CurrentGame.guessWord = CurrentGame.word;
                }
                //W innym wypadku sprawdź czy dana litera występuje
                else if (type.Length == 1 && char.IsLetter(type[0]))
                {
                    if (CheckLetter(type[0]))
                    {
                        CurrentGame.guessWord = AddLetters(type[0]);
                    }
                    //W razie pomyłki zwiększ licznik błędów
                    else
                    {
                        CurrentGame.actualStage++;
                    }
                    //Dodaj literę do listy
                    if (!CurrentGame.guessedLetters.Contains(type[0]))
                    {
                        CurrentGame.guessedLetters.Add(type[0]);
                    }
                }
                CheckEndGame();
            }
            //Generuj wyjście
            output += GenerateOutput();

            if(CurrentGame.gameStarted == false)
            {
                HangmanGames.RemoveAll(p => p.channelId == CurrentGame.channelId);
            }

            //Generuj wyjście
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#5588EE")
            };
            embed.AddField("Hangman", output);

            await ctx.RespondAsync(embed);
        }

        /// <summary>
        /// Ładuję obecną lub tworzy nową grę
        /// </summary>
        /// <param name="channelId">Identyfikator kanału</param>
        private void LoadGame(ulong channelId)
        {
            CurrentGame = HangmanGames.FirstOrDefault(p => p.channelId == channelId);
            if (CurrentGame == null)
            {
                CurrentGame = new HangmanGame(channelId);
                HangmanGames.Add(CurrentGame);
            }
        }

        /// <summary>
        /// Rozpoczyna grę
        /// </summary>
        /// <param name="category">Kategoria</param>
        private void StartGame(string category = null)
        {
            CurrentGame.gameStarted = true;
            CurrentGame.actualStage = 1;
            CurrentGame.word = GetWord(category);
            var word = CurrentGame.word.ToCharArray();
            CurrentGame.guessWord = "";
            CurrentGame.guessedLetters = new List<char>();
            for (var i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(word[i]))
                {
                    CurrentGame.guessWord += "◯";
                }
                else
                {
                    CurrentGame.guessWord += word[i];
                }

            }
        }

        /// <summary>
        /// Losuje słowo
        /// </summary>
        /// <param name="Category">kategoria</param>
        /// <returns></returns>
        private string GetWord(string Category = null)
        {

            using (var databaseContext = new StaticDBContext())
            {
                IQueryable<string> wordList = null;

                if (Category == null)
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.IsDeleted == false)
                        .Select(p => p.Word);
                }
                else if (Category == "państwa")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 1 && p.IsDeleted == false)
                        .Select(p => p.Word);
                }
                else if (Category == "zwierzęta")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 2 && p.IsDeleted == false)
                        .Select(p => p.Word);
                }
                else if (Category == "rzeczy")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 3 && p.IsDeleted == false)
                        .Select(p => p.Word);
                }
                var words = wordList.ToList();

                var wordIndex = _random.Next(0, words.Count);
                var word = words[wordIndex];
                return word;
            }
        }

        /// <summary>
        /// Sprawdza czy litera występuje w słowie
        /// </summary>
        /// <param name="letter">Litera do sprawdzenia</param>
        /// <returns></returns>
        private bool CheckLetter(char letter)
        {
            return CurrentGame.word.ToLower().Contains(letter.ToString());
        }

        /// <summary>
        /// Dodaje literę do ciągu znaków w miejscu w którym występuje w haśle
        /// </summary>
        /// <param name="letter">Litera</param>
        /// <returns></returns>
        private string AddLetters(char letter)
        {
            var guessWord = CurrentGame.guessWord.ToCharArray();
            var word = CurrentGame.word.ToLower();
            for (var i = 0; i < CurrentGame.word.Length; i++)
            {
                if (word[i] == letter)
                {
                    guessWord[i] = CurrentGame.word[i];
                }
            }
            var returnValue = new StringBuilder();
            returnValue.Append(guessWord);
            return returnValue.ToString();
        }

        /// <summary>
        /// Dodaje do wyjścia szybienicę
        /// </summary>
        /// <param name="output"></param>
        private string GenerateOutput()
        {
            var output = "";
            output += CurrentGame.guessWord;
            output += "\n";
            for (var j = 0; j < HangmanConst.Stages[CurrentGame.actualStage - 1].Length; j++)
            {
                output += HangmanConst.Stages[CurrentGame.actualStage - 1][j];
                output += "\n";
            }
            for (var j = 0; j < CurrentGame.guessedLetters.Count; j++)
            {
                output += CurrentGame.guessedLetters[j];
                output += " ";
            }
            output += "\n";
            //W razie wygranej
            if (CurrentGame.guessWord.Equals(CurrentGame.word))
            {
                output += "Wygrana \nSłowo: " + CurrentGame.word;
            }
            //W razie przegranej
            if (CurrentGame.actualStage == Stages)
            {
                output += "Przegrana \nSłowo: " + CurrentGame.word;
            }
            return output;
        }

        /// <summary>
        /// Sprawdź czy gra skończona
        /// </summary>
        private void CheckEndGame()
        {
            if (CurrentGame.guessWord.Equals(CurrentGame.word) || CurrentGame.actualStage == Stages)
            {
                CurrentGame.gameStarted = false;
            }
        }
    }

    public class HangmanGame
    {
        /// <summary>
        /// Id kanału na którym wywołano komendę
        /// </summary>
        public ulong channelId;
        /// <summary>
        /// Czu gra rozpoczęta
        /// </summary>
        public bool gameStarted;
        /// <summary>
        /// Aktualny poziom
        /// </summary>
        public int actualStage;
        /// <summary>
        /// Słowo do odgadnięcia
        /// </summary>
        public string word;
        /// <summary>
        /// Aktualnie odgadywane słowo
        /// </summary>
        public string guessWord;
        /// <summary>
        /// Podane wcześniej litery
        /// </summary>
        public List<char> guessedLetters;

        public HangmanGame(ulong channelId)
        {
            gameStarted = false;
            actualStage = 0;
            word = "";
            guessWord = "";
            guessedLetters = new List<char>();
            this.channelId = channelId;
        }
    }

}
