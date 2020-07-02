using System.Collections.Generic;

namespace Natukaship
{
    public class PriceTierAvailability
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public bool countriesChanged { get; set; }
        public List<CountryPricing> countries { get; set; }
        public bool b2BAppFlagDisabled { get; set; }
        public bool educationDiscountDisabledFlag { get; set; }
        public bool hotFudgeFeatureEnabled { get; set; }
        public bool coldFudgeFeatureEnabled { get; set; }
        public bool bitcodeAutoRecompileDisAllowed { get; set; }
        public bool b2bAppEnabled { get; set; }
        public bool educationalDiscount { get; set; }
        public bool theWorld { get; set; }
        public PricingIntervalsFieldTO pricingIntervalsFieldTO { get; set; }
        public long availableDate { get; set; }
        public int unavailableDate { get; set; }
        public bool creatingApp { get; set; }
        public bool hasApprovedVersion { get; set; }
        public AppVersionsForOTAByPlatforms appVersionsForOTAByPlatforms { get; set; }
        public List<object> b2bUsers { get; set; }
        public List<object> b2bOrganizations { get; set; }
    }

    public class PriceTierAvailabilityResponseObject
    {
        public PriceTierAvailability data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
