using System.Collections.Generic;

namespace Natukaship
{
    public class GetCreateApplication
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public CompanyName companyName { get; set; }
        public VersionString versionString { get; set; }
        public object appRegInfo { get; set; }
        public Dictionary<string, string> bundleIds { get; set; }
        public EnabledPlatformsForCreation enabledPlatformsForCreation { get; set; }
        public ContentProviderId contentProviderId { get; set; }
        public ApplicationName name { get; set; }
        public PrimaryLocaleCode primaryLocaleCode { get; set; }
        public BundleId bundleId { get; set; }
        public BundleIdSuffix bundleIdSuffix { get; set; }
        public VendorId vendorId { get; set; }
        public string initialPlatform { get; set; }
        public int adamId { get; set; }

        public object primaryLanguage { get; set; }
    }

    public class GetCreateApplicationResponseObject
    {
        public GetCreateApplication data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
