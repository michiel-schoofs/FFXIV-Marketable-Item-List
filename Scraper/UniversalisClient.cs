using System.Text.Json;

namespace XIV_Item_Scraper
{
    internal class UniversalisClient : IDisposable
    {
        private readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://universalis.app/api/v2/")};

        public void Dispose()
        {
            client.Dispose();
        }

        public async Task<int[]> GetMarketableItems()
        {
            //Requests https://universalis.app/api/v2/marketable see documentation
            var response = await client.GetAsync("marketable");

            // check that the response is successful and the content is not null otherwise throw an exception
            string jsonResponse = response.IsSuccessStatusCode && response?.Content != null ? await response.Content.ReadAsStringAsync() : "";
            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new Exception("Failed to get marketable items from universalis");
            }

            //response is an array of item ids
            return JsonSerializer.Deserialize<int[]>(jsonResponse) ?? [];
        }
    }
}
