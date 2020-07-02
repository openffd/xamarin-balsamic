using System.Collections.Generic;

namespace Natukaship
{
    public class GetSupportedCountriesResponseObject
    {
        public List<CountryPricing> data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
