namespace Natukaship
{
    public class Territory
    {
        public string code { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public string regionLocaleKey { get; set; }
        public string currencyCodeISO3A { get; set; }

        public string currencyCode => currencyCodeISO3A;

        public static Territory FromCode(string territoryCode)
        {
            // Create a new object based on a two-character country code (e.g. "US" for the United States)
            Territory obj = new Territory
            {
                code = territoryCode
            };

            return obj;
        }
    }
}
