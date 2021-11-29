using System;
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
        public const string searchOrExitMsg = "To view list of songs not found please type V, press Y to try another artist, or N to exit.";

        static async Task Main(string[] args)
        {
            await Home();
        }

        // This method displays the defualt screen.
        private static async Task Home()
        {
            Console.WriteLine("\nPlease enter artist's name:\n");
            var artist = Console.ReadLine();
            Console.WriteLine("\nFetching results. . .");
            await GetRecordingsByArtist(artist);
        }

        // Method takes the artist name passed by user in Home() method & makes API call to MusicBrainz. 
        private static async Task GetRecordingsByArtist(string artist)
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

        // The following method takes the recordings retreieved by GetRecordingsByArtist and the artist name and makes a call 
        // to Lyrics.Ovh API & retrieves lyrics for given artist.
        // Any exceptions are caught and added to a list which a user can optionally view.
        private static async Task GetAverageWordCountByArtistAndTitle(string artist, List<RecordingInformation> result)
        {
            List<int> list = new List<int>();
            List<string> msgList = new List<string>();
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
                        msgList.Add($"\nLyrics for {title} could not be found!");
                    }
                }

            } 

            if (list.Count > 0)
            {
                var average = (int)Queryable.Average(list.AsQueryable());
                if (msgList.Count > 0)
                {
                    Console.WriteLine($"\nBased on {list.Count} results, the average number of words in a song by {artist} is {average}. \n{searchOrExitMsg}.\n");
                    await UserResponseValidator(msgList);
                }
            }
            else
            {

                Console.WriteLine($"\nSorry, no results were found for {artist}! press Y to try another artist, or N to exit.");
                await UserResponseValidator(msgList);
            }
        }

        // This method handles & validates user responses once results have been provided.
        private static async Task UserResponseValidator(List<string> msgList)
        {
            var userInput = Console.ReadLine();
            var positiveInput = userInput.Contains("Y", StringComparison.CurrentCultureIgnoreCase);
            var negativeInput = userInput.Contains("N", StringComparison.CurrentCultureIgnoreCase);
            var viewListInput = userInput.Contains("V", StringComparison.CurrentCultureIgnoreCase);

            while (!positiveInput && !negativeInput && !viewListInput)
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
            else if (viewListInput)
            {
                msgList.ForEach(item => Console.WriteLine(item));
                Console.WriteLine("\nPress Y to try another artist, or N to exit");
                await UserResponseValidator(msgList);
            }
        }

    }

}
