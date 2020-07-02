using System.Collections.Generic;

namespace Natukaship
{
    public class GetAvailableBundleIdsResponseObject
    {
        public string statusCode { get; set; }
        public BundleData data { get; set; }
        public Messages messages { get; set; }
    }

    public class EnabledPlatformsForCreation: CommonSharedValues
    {
        public List<string> value { get; set; }
    }

    public class CompanyName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class ContentProviderId: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class BundleData
    {
        public object vendorId { get; set; }
        public string initialPlatform { get; set; }
        public EnabledPlatformsForCreation enabledPlatformsForCreation { get; set; }
        public BundleId bundleId { get; set; }
        public object versionString { get; set; }
        public object value { get; set; }
        public object bundleIds { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public PrimaryLocaleCode primaryLocaleCode { get; set; }
        public object adamId { get; set; }
        public CompanyName companyName { get; set; }
        public BundleIdSuffix bundleIdSuffix { get; set; }
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public object appRegInfo { get; set; }
        public ContentProviderId contentProviderId { get; set; }
        public object name { get; set; }
    }
}
