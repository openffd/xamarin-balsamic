using System.Collections.Generic;

namespace Natukaship
{
    public class RejectVersionName : CommonSharedValues { }

    public class PrimaryLanguage : CommonSharedValues { }

    public class LargeAppIconValueVersion : PictureValue { }

    public class WatchAppIconValueVersion : PictureValue { }

    public class PrivacyURL : CommonSharedValues
    {
        public string value { get; set; }
    }

    public class AppTrailers : CommonSharedValues
    {
        public AppTrailersValue value { get; set; }
    }

    public class Iphone4AppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class WatchAppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Iphone35AppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Iphone6AppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class IpadAppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Iphone6PlusAppTrailers : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Picture : CommonSharedValues
    {
        public PictureValue value { get; set; }
    }

    public class WatchScreenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class Iphone4Screenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class Iphone35Screenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class IpadScreenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class Iphone6Screenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class SpecificDeviceScreenshots : CommonSharedValues
    {
        public SpecificDeviceScreenshotsValue value { get; set; }
    }

    public class Iphone6PlusScreenshots : CommonSharedValues
    {
        public List<SpecificDeviceScreenshots> value { get; set; }
    }

    public class DeviceScreenshots : CommonSharedValues
    {
        public DeviceScreenshotsValue value { get; set; }
    }

    public class DetailsVersion : CommonSharedValues
    {
        public List<DetailsVersionValue> value { get; set; }
    }

    public class LargeAppIconVersion : LargeAppIcon
    {
        public LargeAppIconValueVersion value { get; set; }
    }

    public class WatchAppIconVersion : WatchAppIcon
    {
        public new WatchAppIconValueVersion value { get; set; }
    }

    public class AppTrailersValue
    {
        public Iphone4AppTrailers iphone4 { get; set; }
        public WatchAppTrailers watch { get; set; }
        public Iphone35AppTrailers iphone35 { get; set; }
        public Iphone6AppTrailers iphone6 { get; set; }
        public IpadAppTrailers ipad { get; set; }
        public Iphone6PlusAppTrailers iphone6Plus { get; set; }
    }

    public class PictureValue
    {
        public string assetToken { get; set; }
        public string url { get; set; }
        public string thumbNailUrl { get; set; }
        public int sortOrder { get; set; }
        public string originalFileName { get; set; }
    }

    public class Newsstand : CommonSharedValues
    {
        public bool isEnabled { get; set; }
        public Picture picture { get; set; }
        public bool isEmptyValue { get; set; }
        public bool pictureEmptyValue { get; set; }
    }

    public class SpecificDeviceScreenshotsValue
    {
        public string assetToken { get; set; }
        public string url { get; set; }
        public string thumbNailUrl { get; set; }
        public int sortOrder { get; set; }
        public string originalFileName { get; set; }
    }

    public class DeviceScreenshotsValue
    {
        public WatchScreenshots watch { get; set; }
        public Iphone4Screenshots iphone4 { get; set; }
        public Iphone35Screenshots iphone35 { get; set; }
        public IpadScreenshots ipad { get; set; }
        public Iphone6Screenshots iphone6 { get; set; }
        public Iphone6PlusScreenshots iphone6Plus { get; set; }
    }

    public class Eula : CommonSharedValues
    {
        public List<object> countries { get; set; }
        public bool isEmptyValue { get; set; }
        public string EULAText { get; set; }
    }

    public class DetailsVersionValue : DetailsValue
    {
        public RejectVersionName name { get; set; }
        public DeviceScreenshots screenshots { get; set; }
        public AppTrailers appTrailers { get; set; }
        public PrivacyURL privacyURL { get; set; }
    }

    public class VersionCommonSharedValues
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public int versionId { get; set; }
        public RejectVersionName name { get; set; }
        public PrimaryLanguage primaryLanguage { get; set; }
        public Version version { get; set; }
        public Copyright copyright { get; set; }
        public PrimaryCategory primaryCategory { get; set; }
        public PrimaryFirstSubCategory primaryFirstSubCategory { get; set; }
        public PrimarySecondSubCategory primarySecondSubCategory { get; set; }
        public SecondaryCategory secondaryCategory { get; set; }
        public SecondaryFirstSubCategory secondaryFirstSubCategory { get; set; }
        public SecondarySecondSubCategory secondarySecondSubCategory { get; set; }
        public SubmittableAddOns submittableAddOns { get; set; }
        public GameCenterSummary gameCenterSummary { get; set; }
        public bool canSendVersionLive { get; set; }
        public bool canPrepareForUpload { get; set; }
        public bool canRejectVersion { get; set; }
        public string status { get; set; }
        public string appType { get; set; }
        public string platform { get; set; }
        public Ratings ratings { get; set; }
        public DetailsVersion details { get; set; }
        public TransitAppFile transitAppFile { get; set; }
        public Eula eula { get; set; }
        public LargeAppIconVersion largeAppIcon { get; set; }
        public WatchAppIconVersion watchAppIcon { get; set; }
        public AppReviewInfo appReviewInfo { get; set; }
        public AppStoreInfo appStoreInfo { get; set; }
        public Dictionary<string, string> appVersionPageLinks { get; set; }
        public PreReleaseBuildVersionString preReleaseBuildVersionString { get; set; }
        public string preReleaseBuildTrainVersionString { get; set; }
        public string preReleaseBuildIconUrl { get; set; }
        public long preReleaseBuildUploadDate { get; set; }
        public bool preReleaseBuildsAreAvailable { get; set; }
        public bool preReleaseBuildIsLegacy { get; set; }
        public bool canBetaTest { get; set; }
        public bool isSaveError { get; set; }
        public bool validationError { get; set; }
        public ReleaseOnApproval releaseOnApproval { get; set; }
        public BundleInfo bundleInfo { get; set; }
        public AutoReleaseDate autoReleaseDate { get; set; }
    }

    public class RejectVersion : VersionCommonSharedValues
    {
        public Newsstand newsstand { get; set; }
    }

    public class RejectVersionResponseObject
    {
        public RejectVersion data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
