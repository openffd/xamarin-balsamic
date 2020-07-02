using System.Collections.Generic;

namespace Natukaship
{
    public class PricingMatrixResponseObject
    {
        public PricingMatrix data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }

    public class PricingMatrix
    {
        public List<PricingTier> pricingTiers { get; set; }
        public Dictionary<string, string> countryCurrencyMap { get; set; }
    }

    public class PricingTier
    {
        public string tierStem { get; set; }
        public string tierName { get; set; }
        public int sortOrder { get; set; }
        public List<PricingInfo> pricingInfo { get; set; }
    }

    public class PricingInfo
    {
        public string countryCode { get; set; }
        public string currencyCode  { get; set; }
        public int wholesalePrice  { get; set; }
        public int wholesalePrice2  { get; set; }
        public int retailPrice  { get; set; }
        public string country  { get; set; }
        public string currencySymbol  { get; set; }
        public string fRetailPrice  { get; set; }
        public string fWholesalePrice  { get; set; }
        public string fWholesalePrice2  { get; set; }
    }
}
