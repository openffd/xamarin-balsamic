using System.Collections.Generic;

namespace Natukaship
{
    public class VersionInfo : VersionCommonSharedValues
    {
        public object value { get; set; }
        public PhasedRelease phasedRelease { get; set; }
        public string externalAppReviewState { get; set; }
        public ImageSharedValues largeMessagesIcon { get; set; }
        public object preReleaseBuild { get; set; }
        public object marketingOptInEnabled { get; set; }
        public RatingsReset ratingsReset { get; set; }
        public bool isZulu { get; set; }
        public ImageSharedValues atvHomeScreenIcon { get; set; }
        public ImageSharedValues atvTopShelfIcon { get; set; }
    }

    public class AvailableOnFrenchStore : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class CcatFile : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class ContainsProprietaryCryptography : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class ContainsThirdPartyCryptography : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class IsExempt : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class UsesEncryption : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class ExportCompliance
    {
        public string appType { get; set; }
        public AvailableOnFrenchStore availableOnFrenchStore { get; set; }
        public CcatFile ccatFile { get; set; }
        public ContainsProprietaryCryptography containsProprietaryCryptography { get; set; }
        public ContainsThirdPartyCryptography containsThirdPartyCryptography { get; set; }
        public object encryptionUpdated { get; set; }
        public bool exportComplianceRequired { get; set; }
        public IsExempt isExempt { get; set; }
        public string platform { get; set; }
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public UsesEncryption usesEncryption { get; set; }
    }

    public class ContainsThirdPartyContent : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class HasRights : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class ContentRights
    {
        public ContainsThirdPartyContent containsThirdPartyContent { get; set; }
        public HasRights hasRights { get; set; }
    }

    public class UsesIdfa : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class ServesAds : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class TracksInstall : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class TracksAction : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class LimitsTracking : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class AdIdInfo
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public UsesIdfa usesIdfa { get; set; }
        public ServesAds servesAds { get; set; }
        public TracksInstall tracksInstall { get; set; }
        public TracksAction tracksAction { get; set; }
        public LimitsTracking limitsTracking { get; set; }
    }

    public class VersionInfoData
    {
        public VersionInfo versionInfo { get; set; }
        public ExportCompliance exportCompliance { get; set; }
        public ContentRights contentRights { get; set; }
        public AdIdInfo adIdInfo { get; set; }
        public object previousPurchaseRestrictions { get; set; }
        public List<object> availableExportCompliances { get; set; }
    }

    public class AppSubmissionResponseObject
    {
        public VersionInfoData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
