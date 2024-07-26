namespace XIV_Item_Scraper
{
    internal class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name_de { get; set; }
        public string Name_en { get; set; }
        public string Name_fr { get; set; }
        public string Name_ja { get; set; }

        public override string ToString()
        {
            return $"{ID},{Name_en},{Name_de},{Name_fr},{Name_ja}";
        }
    }
}
