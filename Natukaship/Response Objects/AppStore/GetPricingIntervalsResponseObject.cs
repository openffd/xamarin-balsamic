using System.Collections.Generic;

namespace Natukaship
{
    public class CountryPricing
    {
        public string code { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public string regionLocaleKey { get; set; }
        public string currencyCodeISO3A { get; set; }
    }

    public class PricingIntervalsFieldTO : CommonSharedValues
    {
        public List<PricingIntervalsFieldTOValue> value { get; set; }
    }

    public class ClearedForPreOrder : CommonSharedValues
    {
        public bool value { get; set; }
    }

    public class AppAvailableDate : CommonSharedValues
    {
        public string value { get; set; }
    }

    public class PreOrder
    {
        public ClearedForPreOrder clearedForPreOrder { get; set; }
        public AppAvailableDate appAvailableDate { get; set; }
        public object preOrderAvailableDate { get; set; }
    }

    public class OS
    {
        public string versionId { get; set; }
        public string version { get; set; }
        public bool restoreKillSwitch { get; set; }
        public string createDate { get; set; }
        public bool otaRestoreKilled { get; set; }
    }

    public class AppVersionsForOTAByPlatforms
    {
        public List<OS> iOS { get; set; }
    }

    public class B2bUserValue
    {
        public bool add { get; set; }
        public bool delete { get; set; }
        public string dsUsername { get; set; }
    }

    public class B2bUser
    {
        public B2bUserValue value { get; set; }
    }

    public class B2bOrganizationValue
    {
        public string type { get; set; }
        public string depCustomerId { get; set; }
        public string organizationId { get; set; }
        public string name { get; set; }
    }

    public class B2bOrganization
    {
        public B2bOrganizationValue value { get; set; }
    }

    public class PricingIntervalsData : AvailabilityRawData { }

    public class GetPricingIntervalsResponseObject
    {
        public PricingIntervalsData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }

    public class PricingIntervalsFieldTOValue
    {
        public object priceTierEffectiveDate { get; set; }
        public object priceTierEndDate { get; set; }
        public object priceTierEffectiveISODate { get; set; }
        public object priceTierEndISODate { get; set; }
        public string tierStem { get; set; }
    }
}
