﻿using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Plugins.Anime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaBrowser.Plugins.Anime.Providers
{
    public static class GenreHelper
    {
        private static readonly Dictionary<string, string> GenreMappings = new Dictionary<string, string>
        {
            {"Action", "Action"},
            {"Advanture", "Adventure"},
            {"Contemporary Fantasy", "Fantasy"},
            {"Comedy", "Comedy"},
            {"Dark Fantasy", "Fantasy"},
            {"Dementia", "Psychological Thriller"},
            {"Demons", "Fantasy"},
            {"Drama", "Drama"},
            {"Ecchi", "Ecchi"},
            {"Fantasy", "Fantasy"},
            {"Harem", "Harem"},
            {"Hentai", "Adult"},
            {"Historical", "Period & Historical"},
            {"Horror", "Horror"},
            {"Josei", "Josei"},
            {"Kids", "Kids"},
            {"Magic", "Fantasy"},
            {"Martial Arts", "Martial Arts"},
            {"Mahou Shoujo", "Mahou Shoujo"},
            {"Mecha", "Mecha"},
            {"Music", "Music"},
            {"Mystery", "Mystery"},
            {"Parody", "Comedy"},
            {"Psychological", "Psychological Thriller"},
            {"Romance", "Romance"},
            {"Sci-Fi", "Sci-Fi"},
            {"Seinen", "Seinen"},
            {"Shoujo", "Shoujo"},
            {"Shounen", "Shounen"},
            {"Slice of Life", "Slice of Life"},
            {"Space", "Sci-Fi"},
            {"Sports", "Sport"},
            {"Supernatural", "Supernatural"},
            {"Thriller", "Thriller"},
            {"Tragedy", "Tragedy"},
            {"Witch", "Supernatural"},
            {"Vampire", "Supernatural"},
            {"Yaoi", "Adult"},
            {"Yuri", "Adult"},
            {"Zombie", "Supernatural"},
            //AniSearch Genre
            {"Geister­geschichten", "Geister­geschichten"},
            {"Romanze", "Romance"},
            {"Alltagsdrama", "Slice of Life"},
            {"Alltagsleben", "Slice of Life"},
            {"Psychodrama", "Psycho"},
            {"Actiondrama", "Action"},
            {"Nonsense-Komödie", "Comedy"},
            {"Magie", "Fantasy"},
            {"Abenteuer", "Adventure"},
            {"Komödie", "Comedy"},
            {"Erotik", "Adult"},
            {"Historisch", "Period & Historical"},
            //Proxer
            {"Slice_of_Life", "Slice of Life"},
        };

        private static readonly string[] GenresAsTags =
        {
            "Hentai",
            "Space",
            "Weltraum",
            "Yaoi",
            "Yuri",
            "Demons",
            "Witch",
            //AniSearchTags
            "Krieg",
            "Militär",
            "Satire",
            "Übermäßige Gewaltdarstellung",
            "Monster",
            "Zeitgenössische Fantasy",
            "Dialogwitz",
            "Romantische Komödie",
            "Slapstick",
            "Alternative Welt",
            "4-panel",
            "CG-Anime",
            "Episodisch",
            "Moe",
            "Parodie",
            "Splatter",
            "Tragödie",
            "Verworrene Handlung",
            //Themen
            "Erwachsenwerden",
            "Gender Bender",
            "Ältere Frau, jüngerer Mann",
            "Älterer Mann, jüngere Frau",
            //Schule (School)
            "Grundschule",
            "Kindergarten",
            "Klubs",
            "Mittelschule",
            "Oberschule",
            "Schule",
            "Universität",
            //Zeit (Time)
            "Altes Asien",
            "Frühe Neuzeit",
            "Gegenwart",
            "industrialisierung",
            "Meiji-Ära",
            "Mittelalter",
            "Weltkriege",
            //Fantasy
            "Dunkle Fantasy",
            "Epische Fantasy",
            "Zeitgenössische Fantasy",
            //Ort
            "Alternative Welt",
            "In einem Raumschiff",
            "Weltraum",
            //Setting
            "Cyberpunk",
            "Endzeit",
            "Space Opera",
            //Hauptfigur
            "Charakterschache Heldin",
            "Charakterschacher Held",
            "Charakterstarke Heldin",
            "Charakterstarker Held",
            "Gedächtnisverlust",
            "Stoische Heldin",
            "Stoischer Held",
            "Widerwillige Heldin",
            "Widerwilliger Held",
            //Figuren
            "Diva",
            "Genie",
            "Schul-Delinquent",
            "Tomboy",
            "Tsundere",
            "Yandere",
            //Kampf (fight)
            "Bionische Kräfte",
            "Martial Arts",
            "PSI-Kräfte",
            "Real Robots",
            "Super Robots",
            "Schusswaffen",
            "Schwerter & co",
            //Sports (Sport)
            "Baseball",
            "Boxen",
            "Denk- und Glücksspiele",
            "Football",
            "Fußball",
            "Kampfsport",
            "Rennsport",
            "Tennis",
            //Kunst (Art)
            "Anime & Film",
            "Malerei",
            "Manga & Doujinshi",
            "Musik",
            "Theater",
            //Tätigkeit
            "Band",
            "Detektiv",
            "Dieb",
            "Essenszubereitung",
            "Idol",
            "Kopfgeldjäger",
            "Ninja",
            "Polizist",
            "Ritter",
            "Samurai",
            "Solosänger",
            //Wesen
            "Außerirdische",
            "Cyborgs",
            "Dämonen",
            "Elfen",
            "Geister",
            "Hexen",
            "Himmlische Wesen",
            "Kamis",
            "Kemonomimi",
            "Monster",
            "Roboter & Androiden",
            "Tiermenschen",
            "Vampire",
            "Youkai",
            "Zombie",
            //Proxer
            "Virtual Reality",
            "Game",
            "Survival",
            "Fanservice",
            "Schlauer Protagonist",
        };

        private static readonly Dictionary<string, string> IgnoreIfPresent = new Dictionary<string, string>
        {
            {"Psychological Thriller", "Thriller"}
        };

        public static void CleanupGenres(Series series)
        {
            PluginConfiguration config = Plugin.Instance.Configuration;

            if (config.TidyGenreList)
            {
                series.Genres = RemoveRedundantGenres(series.Genres)
                                           .Distinct()
                                           .ToList();

                TidyGenres(series);
            }

            var max = config.MaxGenres;
            if (config.AddAnimeGenre)
            {
                series.Genres.Remove("Animation");
                series.Genres.Remove("Anime");

                max = Math.Max(max - 1, 0);
            }

            if (config.MaxGenres > 0)
            {
                series.Genres = series.Genres.Take(max).ToList();
            }

            if (!series.Genres.Contains("Anime") && config.AddAnimeGenre)
            {
                if (series.Genres.Contains("Animation"))
                    series.Genres.Remove("Animation");

                series.AddGenre("Anime");
            }

            series.Genres.Sort();
        }

        public static void TidyGenres(Series series)
        {
            var config = Plugin.Instance.Configuration;

            var genres = new HashSet<string>();
            var tags = new HashSet<string>(series.Tags);

            foreach (string genre in series.Genres)
            {
                string mapped;
                if (GenreMappings.TryGetValue(genre, out mapped))
                    genres.Add(mapped);
                else
                {
                    genres.Add(genre);
                }

                if (GenresAsTags.Contains(genre))
                {
                    genres.Add(genre);
                }
            }

            series.Genres = genres.ToList();
            series.Tags = tags.ToArray();
        }

        public static IEnumerable<string> RemoveRedundantGenres(IEnumerable<string> genres)
        {
            var list = genres as IList<string> ?? genres.ToList();

            var toRemove = list.Where(IgnoreIfPresent.ContainsKey).Select(genre => IgnoreIfPresent[genre]).ToList();
            return list.Where(genre => !toRemove.Contains(genre));
        }
    }
}