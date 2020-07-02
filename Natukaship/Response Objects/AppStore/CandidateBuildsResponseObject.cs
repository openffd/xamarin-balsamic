using System.Collections.Generic;

namespace Natukaship
{
    public class Build
    {
        public string type { get; set; }
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public string buildVersion { get; set; }
        public string trainVersion { get; set; }
        public object uploadDate { get; set; }
        public string iconUrl { get; set; }
        public string iconAssetToken { get; set; }
        public string appName { get; set; }
        public string platform { get; set; }
        public bool betaEntitled { get; set; }
        public bool exceededFileSizeLimit { get; set; }
        public bool wentLiveWithVersion { get; set; }
        public bool processing { get; set; }
        public string state { get; set; }
        public List<string> supportedHardware { get; set; }
        public object largeAppIcon { get; set; }
        public ImageSharedValues atvTopShelfIcon { get; set; }
        public ImageSharedValues atvHomeScreenIcon { get; set; }
        public int issuesCount { get; set; }
        public string version { get; set; }
    }

    public class CandidateBuilds
    {
        public List<Build> builds { get; set; }
    }

    public class CandidateBuildsResponseObject
    {
        public CandidateBuilds data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
