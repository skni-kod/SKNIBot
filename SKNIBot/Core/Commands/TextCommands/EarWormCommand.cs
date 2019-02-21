using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models;
using SKNIBot.Core.Settings;
using SpotifyWebApi;
using SpotifyWebApi.Api;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model;
using SpotifyWebApi.Model.Enum;
using SpotifyWebApi.Model.Uri;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Różne")]
    class EarWormCommand : BaseCommandModule
    {
        [Command("earworm")]
        [Description("Sprawdź robaka dnia/tygodnia/miesiąca/roku")]
        public async Task CheckEarWorm(CommandContext ctx, [Description("Okres czasu")] string period = null)
        {
            await ctx.TriggerTypingAsync();
            if (period == null)
            {
                await ctx.RespondAsync("Składnia to `!earworm {Okres}`");
                return;
            }

            string message;

            switch(period.ToLower())
            {
                case "today":
                    message = getToday();
                    break;
                case "week":
                    message = getWeek();
                    break;
                case "month":
                    message = getMonth();
                    break;
                case "year":
                    message = getYear();
                    break;
                default:
                    message = "Dostępne okresy czasu to: today, week, month, year";
                    return;
            }

            await ctx.RespondAsync(message);

            //await ctx.RespondAsync("Czytaj tego loga a nie się lampisz");
           //var artists = track.Track.Artists.Select(x => x.Name);
           //await ctx.RespondAsync(String.Format("{0} - {1}", track.Track.Name, String.Join(", ", artists)));

        }

        private string getToday()
        {
            using (StaticDBContext db = new StaticDBContext())
            {
                var track = db.SpotifyEarWorms.Where(e => e.Category == TypeOfPeriod.DAY).OrderByDescending(e => e.DayStamp).FirstOrDefault();
                if (track == null)
                {
                    return "Earworm dnia: " + newSong(db, TypeOfPeriod.DAY);
                }     
                if(track.DayStamp.DayOfYear < DateTime.Now.DayOfYear)
                {
                    return "Earworm dnia: " + newSong(db, TypeOfPeriod.DAY);
                }
                return String.Format("Earworm dnia: {0} - {1}", track.Title, track.Author);
            }
        }

        private string getWeek()
        {
            using (StaticDBContext db = new StaticDBContext())
            {
                var track = db.SpotifyEarWorms.Where(e => e.Category == TypeOfPeriod.WEEK).OrderByDescending(e => e.DayStamp).FirstOrDefault();
                if (track == null)
                {
                    return "Earworm tygodnia: " +  newSong(db, TypeOfPeriod.WEEK);
                }
                if ((getWeekNumber(track.DayStamp) < getWeekNumber(DateTime.Now)) || (track.DayStamp.Year < DateTime.Now.Year))
                {
                    return "Earworm tygodnia: " + newSong(db, TypeOfPeriod.WEEK);
                }
                return String.Format("Earworm tygodnia: {0} - {1}", track.Title, track.Author);
            }
        }

        private string getMonth()
        {
            using (StaticDBContext db = new StaticDBContext())
            {
                var track = db.SpotifyEarWorms.Where(e => e.Category == TypeOfPeriod.MONTH).OrderByDescending(e => e.DayStamp).FirstOrDefault();
                if (track == null)
                {
                    return "Earworm miesiąca: " + newSong(db, TypeOfPeriod.MONTH);
                }
                if ((track.DayStamp.Month < DateTime.Now.Month) || (track.DayStamp.Year < DateTime.Now.Year))
                {
                    return "Earworm miesiąca: " + newSong(db, TypeOfPeriod.MONTH);
                }
                return String.Format("Earworm miesiąca: {0} - {1}", track.Title, track.Author);
            }
        }

        private string getYear()
        {
            using (StaticDBContext db = new StaticDBContext())
            {
                var track = db.SpotifyEarWorms.Where(e => e.Category == TypeOfPeriod.YEAR).OrderByDescending(e => e.DayStamp).FirstOrDefault();
                if (track == null)
                {
                    return "Earworm roku: " + newSong(db, TypeOfPeriod.YEAR);
                }
                if (track.DayStamp.Year < DateTime.Now.Year)
                {
                    return "Earworm roku: " + newSong(db, TypeOfPeriod.YEAR);
                }
                return String.Format("Earworm roku: {0} - {1}", track.Title, track.Author);
            }
        }

        private string newSong(StaticDBContext db, TypeOfPeriod typeOfPeriod)
        {
            var newTrack = getSongFromApi();
            SpotifyEarWorm earWorm = new SpotifyEarWorm
            {
                Title = newTrack.Track.Name,
                Author = String.Join(", ", newTrack.Track.Artists.Select(x => x.Name)),
                DayStamp = DateTime.Now,
                Category = typeOfPeriod
            };
            db.Add(earWorm);
            db.SaveChanges();
            return String.Format("{0} - {1}", earWorm.Title, earWorm.Author);
        }

        private PlaylistTrack getSongFromApi()
        {
            var param = new AuthParameters
            {
                Scopes = Scope.All,
                ClientId = SettingsLoader.Container.Spotify_Client_Id,
                ClientSecret = SettingsLoader.Container.Spotify_Client_Secret,
                RedirectUri = "http://google.com",
                ShowDialog = false, // Set to true to login each time.
            };

            var token = ClientCredentials.GetToken(param);
            ISpotifyWebApi api = new SpotifyWebApi.SpotifyWebApi(token);


            FullPlaylist playlist = api.Playlist.GetPlaylist(SpotifyUri.Make("spotify:user:1189618539:playlist:03gha5BWxoJ17xtn3FOvyl")).Result;
            return playlist.Tracks.Items.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        private int getWeekNumber(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }
}
