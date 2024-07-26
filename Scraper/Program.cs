using System.Collections.Concurrent;
using System.Diagnostics;

namespace XIV_Item_Scraper
{
    internal class Program
    {
        //output file
        private const string _outputFile = "output.csv";
        //max number of threads to request XIV API
        private static int _numThreads = 10;
        public const char Delimiter = ';';

        static  void Main(string[] args)
        {
            //all item ids
            int[] itemIds;
            //intiliase api clients
            XIVClient _xivClient = new();
            using(UniversalisClient _universalisClient = new())
            {
                //Get marketable item id list
                itemIds = _universalisClient.GetMarketableItems().Result;
                //prevent duplicate item ids
                itemIds = itemIds.Distinct().ToArray();
            }

            //Concurrent dictionary to store item data (needs to be concurrent as we are using parallel for each)
            ConcurrentBag<Item> items = new ConcurrentBag<Item>();


            //Get item data from XIV API
            Parallel.ForEach(itemIds, new ParallelOptions { MaxDegreeOfParallelism = _numThreads }, itemId =>
            {
                try {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    //Get item data from XIV API
                    Item item = _xivClient.GetItemName(itemId).Result;
                    //Add item to List
                    items.Add(item);
                    if (sw.ElapsedMilliseconds < 1000)
                    {
                        //since our max is 10 threads we want every thread to send out a max of 1 request every 500ms
                        //this is to prevent rate limiting from XIV API (20 requests per second)
                        //the api sometimes still bottle necks me so I add a 500ms buffer effectively slowing us down to 10 requests per second :/
                        Thread.Sleep(1000 - (int)sw.ElapsedMilliseconds);
                    }
                }catch{}
            });

            //output dictionary to file
            string fileContent = string.Join('\n', items);
            string[] headers= new string[] { "ID", "Name_EN", "Name_DE", "Name_FR", "Name_JA" };
            File.WriteAllText(_outputFile, $"{string.Join(Delimiter, headers)}\n{fileContent}");      
        }
    }
}
