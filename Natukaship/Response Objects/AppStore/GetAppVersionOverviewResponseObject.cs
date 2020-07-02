using System.Collections.Generic;

namespace Natukaship
{
    public class AppVersionLocalizedMetadata
    {
        public string localeCode { get; set; }
        public string name { get; set; }
    }

    public class Platform
    {
        public string type { get; set; }
        public string platformString { get; set; }
        public bool everBeenOnSale { get; set; }
        public VersionSetType inFlightVersion { get; set; }
        public VersionSetType deliverableVersion { get; set; }
    }

    public class RcIssue
    {
        public string link { get; set; }
        public string platform { get; set; }
    }

    public class AppVersionOverviewData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public string adamId { get; set; }
        public string primaryLocaleCode { get; set; }
        public List<string> features { get; set; }
        public object appTransferState { get; set; }
        public List<AppVersionLocalizedMetadata> localizedMetadata { get; set; }
        public List<Platform> platforms { get; set; }
        public List<string> allowedNewVersionPlatforms { get; set; }
        public List<string> submittablePlatforms { get; set; }
        public List<string> deleteablePlatforms { get; set; }
        public string appStoreUrl { get; set; }
        public string analyticsUrl { get; set; }
        public string salesAndTrendsUrl { get; set; }
        public string salesAndTrendsBaseUrl { get; set; }
        public bool isPreorderLive { get; set; }
        public bool requiresPreorderRelease { get; set; }
        public bool isAAG { get; set; }
        public bool isFirstParty { get; set; }
        public bool canCreateAddOn { get; set; }
        public bool isIAPLimitExceeded { get; set; }
        public bool hasAppTransferRequest { get; set; }
        public bool hasValidContract { get; set; }
        public object archiveTimestamp { get; set; }
        public bool isOrEverWasAKidsApp { get; set; }
        public int recurringAddOnCount { get; set; }
        public List<object> contractAnnouncements { get; set; }
        public List<RcIssue> rcIssues { get; set; }
        public bool everSubmittedFreeSubscriptionAddon { get; set; }
        public bool everSubmittedAnAddon { get; set; }
        public int freeSubscriptionAddonCount { get; set; }
    }

    public class GetAppVersionOverviewResponseObject
    {
        public AppVersionOverviewData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
