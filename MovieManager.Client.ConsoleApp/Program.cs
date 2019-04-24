using ConsoleTableExt;
using MovieManager.Core.DataTransferObjects;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;

namespace MovieManager.Client.ConsoleApp
{
    /// <summary>
    /// Test for the WebApi
    ///     - https://github.com/restsharp/RestSharp/wiki
    ///     - https://github.com/minhhungit/ConsoleTableExt
    /// </summary>
    class Program
    {
        private const string ServerWithPort = "localhost:54026";

        static void Main()
        {
            RetrieveCategories();
            RetrieveMoviesForCategoryId(3);
        }

        private static void RetrieveMoviesForCategoryId(int id)
        {
            var client = new RestClient($"http://{ServerWithPort}");
            var request = new RestRequest($"api/categories/{id}/movies", DataFormat.Json);
            var response = client.Get(request);

            JArray movies = JArray.Parse(response.Content);

            Console.WriteLine();
            Console.WriteLine("Movies for Category 3:");
            Console.WriteLine("======================");

            ConsoleTableBuilder
                .From(
                    movies
                        .Select(m => m.ToObject<MovieDTO>())
                        .OrderBy(m => m.Title)
                        .ToList())
                .ExportAndWriteLine();
        }

        private static void RetrieveCategories()
        {
            var client = new RestClient($"http://{ServerWithPort}");
            var request = new RestRequest("api/categories", DataFormat.Json);
            var response = client.Get(request);

            JArray categories = JArray.Parse(response.Content);

            Console.WriteLine();
            Console.WriteLine("Categories:");
            Console.WriteLine("===========");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }

        }
    }
}
