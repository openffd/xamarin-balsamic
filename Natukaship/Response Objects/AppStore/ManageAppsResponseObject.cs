using System.Collections.Generic;

namespace Natukaship
{
    public class ManageAppsResponseObject
    {
        public string statusCode { get; set; }
        public AppSummaries data { get; set; }
        public Messages messages { get; set; }
    }

    public class VersionSetType
    {
        public bool hasGrievous { get; set; }
        public bool isZulu { get; set; }
        public int issuesCount { get; set; }
        public string id { get; set; }
        public List<string> supportedHardware { get; set; }
        public LargeAppIcon largeAppIcon { get; set; }
        public string version { get; set; }
        public string type { get; set; }
        public string stateKey { get; set; }
        public string state { get; set; }
        public string stateGroup { get; set; }
        public bool isWatchCombo { get; set; }
    }

    public class AppSummaries
    {
        public int removedAppCount { get; set; }
        public bool showSharedSecret { get; set; }
        public bool macBundlesEnabled { get; set; }
        public bool canCreateMacApps { get; set; }
        public bool cloudStorageEnabled { get; set; }
        public string sharedSecretLink { get; set; }
        public List<object> contractAnnouncements { get; set; }
        public string gameCenterGroupLink { get; set; }
        public List<string> enabledPlatforms { get; set; }
        public string cloudStorageLink { get; set; }
        public string catalogReportsLink { get; set; }
        public bool canCreateIOSApps { get; set; }
        public List<Application> summaries { get; set; }
        public bool canCreateAppBundles { get; set; }
    }
}
