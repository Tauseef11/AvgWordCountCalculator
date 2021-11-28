﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AvgWordCountCalculator
{
    public class AverageWordCountCalculator
    {
        private static readonly HttpClient client = new HttpClient();
        public const string searchOrExitMsg = "Please press Y to try another artist or N to exit.";

        static async Task Main(string[] args)
        {
            await Home();
        }

        public static async Task Home()
        {
            Console.WriteLine("\nPlease enter artist's name:\n");
            var artist = Console.ReadLine();
            await GetRecordingsByArtist(artist);
        }

        public static async Task GetRecordingsByArtist(string artist)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("user-agent", "Airelogic/1.0.0 ( tauseef494@gmail.com )");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetStringAsync($"https://musicbrainz.org/ws/2/recording/?query=artistname:{artist}");

            var msg = await response;

            Recordings result = JsonConvert.DeserializeObject<Recordings>(msg);

            await GetAverageWordCountByArtistAndTitle(artist, result.recordings);

        }

        public static async Task GetAverageWordCountByArtistAndTitle(string artist, List<RecordingInformation> result)
        {
            List<int> list = new List<int>();
            var title = " ";

            foreach (var recording in result)
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    title = recording.title;

                    var response = client.GetStringAsync($"https://api.lyrics.ovh/v1/{artist}/{title}");
                    var msg = await response;

                    var delimiters = new char[] { ' ', '\r', '\n' };
                    var msgLength = msg.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

                    list.Add(msgLength);


                }
                catch (Exception e)
                {
                    if (e.Message != null)
                    {
                        Console.WriteLine($"\nLyrics for {title} could not be found!");
                    }
                }

            }

            if (list.Count > 0)
            {
                var average = (int)Queryable.Average(list.AsQueryable());

                Console.WriteLine($"\nBased on {list.Count} results, the average number of words in a song by {artist} is {average}. \n{searchOrExitMsg}\n");
                await UserResponseValidator();
            }
            else
            {

                Console.WriteLine($"\nSorry, no results were found for {artist}! \n{searchOrExitMsg}\n");
                await UserResponseValidator();
            }
        }

        public static async Task UserResponseValidator()
        {
            var userInput = Console.ReadLine();
            var positiveInput = userInput.Contains("Y", StringComparison.CurrentCultureIgnoreCase);
            var negativeInput = userInput.Contains("N", StringComparison.CurrentCultureIgnoreCase);

            while (!positiveInput && !negativeInput)
            {
                Console.WriteLine($"\nInvalid input. {searchOrExitMsg}\n");

                userInput = Console.ReadLine();
                positiveInput = userInput.Contains("Y", StringComparison.CurrentCultureIgnoreCase);
                negativeInput = userInput.Contains("N", StringComparison.CurrentCultureIgnoreCase);
            }

            if (positiveInput)
            {
                Console.Clear();
                await Home();
            }
            else if (negativeInput)
            {
                Environment.Exit(0);
            }
        }

    }

}