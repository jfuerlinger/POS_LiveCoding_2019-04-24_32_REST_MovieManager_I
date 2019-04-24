using Newtonsoft.Json.Linq;
using RestSharp;
using System;

namespace MovieManager.ConsoleClient
{
    class Program
    {
        private const string ServerNameWithPort = "localhost:44307";

        static void Main(string[] args)
        {
            RetrieveCategories();
        }

        public static void RetrieveCategories()
        {
            var client = new RestClient($"https://{ServerNameWithPort}");
            var request = new RestRequest("api/categories", DataFormat.Json);

            var response = client.Get(request);


            JArray categories = JArray.Parse(response.Content);

            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }

            Console.ReadKey();
        }
    }
}
