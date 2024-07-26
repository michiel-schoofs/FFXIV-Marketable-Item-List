using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XIV_Item_Scraper
{
    internal class XIVClient : IDisposable
    {  
        private readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://xivapi.com/")};

        public void Dispose()
        {
            client.Dispose();
        }

        public async Task<Item> GetItemName(int itemId)
        {
            var response = await client.GetAsync($"item/{itemId}");

            // check that the response is successful and the content is not null otherwise throw an exception
            string jsonResponse = response.IsSuccessStatusCode && response?.Content != null ? await response.Content.ReadAsStringAsync() : "";
            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new Exception($"Failed to get item {itemId} from XIV API");
            }

            Item? result = JsonSerializer.Deserialize<Item>(jsonResponse) ?? null;
            return result == null ? throw new Exception($"Invalid Json Returned") : result;
        }
    }
}
