using System.Collections.Generic;

namespace Natukaship
{
    public class SubmittableAddOns: CommonSharedValues 
    {
        public List<object> value { get; set; }
    }

    public class Leaderboards: CommonSharedValues
    {
        public List<object> value { get; set; }
    }

    public class DisplaySets: CommonSharedValues
    {
        public List<object> value { get; set; }
    }

    public class Achievements: CommonSharedValues
    {
        public List<object> value { get; set; }
    }

    public class VersionCompatibility: CommonSharedValues
    {
        public List<VersionCompatibilityValue> value { get; set; }
    }

    public class State: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class TransitAppFile: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class FirstName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class LastName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class ContactPhoneNumber: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class EmailAddress: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class ReviewNotes: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class UserName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Password: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class EntitlementUsages: CommonSharedValues
    {
        public List<object> value { get; set; }
    }

    public class TradeName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class AddressLine1: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class AddressLine2: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class AddressLine3: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class CityName: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class PostalCode: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Country: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class ShouldDisplayInStore: CommonSharedValues
    {
        public bool value { get; set; }
    }

    public class ReleaseOnApproval: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class BundleInfo
    {
        public bool supportsAppleWatch { get; set; }
    }

    public class AutoReleaseDate: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Version: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Copyright: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class SecondaryFirstSubCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Description: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class ReleaseNotes: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Keywords: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Screenshots: CommonSharedValues
    {
        public List<ScreenshotValue> value { get; set; }
    }

    public class MarketingUrl: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class SupportUrl: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class Details: CommonSharedValues
    {
        public List<DetailsValue> value { get; set; }
    }

    public class LargeAppIconValue: CommonSharedValues
    {
        public LargeAppIcon value { get; set; }
    }

    public class WatchAppIcon: CommonSharedValues
    {
        public ImageSharedValues value { get; set; }
    }

    public class PreReleaseBuildVersionString: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class VersionCompatibilityValue
    {
        public List<Platform> platforms { get; set; }
        public string name { get; set; }
        public string adamId { get; set; }
        public string iconAssetToken { get; set; }
    }

    public class GameCenterSummary: CommonSharedValues
    {
        public Leaderboards leaderboards { get; set; }
        public DisplaySets displaySets { get; set; }
        public Achievements achievements { get; set; }
        public VersionCompatibility versionCompatibility { get; set; }
        public int usedLeaderboards { get; set; }
        public int maxLeaderboards { get; set; }
        public int usedLeaderboardSets { get; set; }
        public int maxLeaderboardSets { get; set; }
        public int usedAchievementPoints { get; set; }
        public int maxAchievementPoints { get; set; }
        public bool isEnabled { get; set; }
        public List<object> unapprovedAchievements { get; set; }
        public List<object> unapprovedLeaderboards { get; set; }
        public List<object> unapprovedLeaderboardSets { get; set; }
        public bool isEmptyValue { get; set; }
    }

    public class PhasedRelease
    {
        public State state { get; set; }
        public object startDate { get; set; }
        public object lastPaused { get; set; }
        public object pausedDuration { get; set; }
        public int totalPauseDays { get; set; }
        public object currentDayNumber { get; set; }
        public Dictionary<string, int> dayPercentageMap { get; set; }
        public bool isEnabled { get; set; }
    }

    public class AccountRequired
    {
        public bool value { get; set; }
        public bool isEditable { get; set; }
        public bool isRequired { get; set; }
        public List<string> errorKeys { get; set; }
    }

    public class AttachmentFile
    {
        public string assetToken { get; set; }
        public string name { get; set; }
        public string fileType { get; set; }
        public string url { get; set; }
    }

    public class AttachmentFiles : CommonSharedValues
    {
        public AttachmentFile value { get; set; }
    }

    public class AppReviewInfo
    {
        public FirstName firstName { get; set; }
        public LastName lastName { get; set; }
        public ContactPhoneNumber phoneNumber { get; set; }
        public EmailAddress emailAddress { get; set; }
        public ReviewNotes reviewNotes { get; set; }
        public UserName userName { get; set; }
        public Password password { get; set; }
        public AccountRequired accountRequired { get; set; }
        public EntitlementUsages entitlementUsages { get; set; }
        public AttachmentFiles attachmentFiles { get; set; }
    }

    public class AppStoreInfo
    {
        public TradeName tradeName { get; set; }
        public FirstName firstName { get; set; }
        public LastName lastName { get; set; }
        public ContactPhoneNumber phoneNumber { get; set; }
        public EmailAddress emailAddress { get; set; }
        public object appRegInfo { get; set; }
        public AddressLine1 addressLine1 { get; set; }
        public AddressLine2 addressLine2 { get; set; }
        public AddressLine3 addressLine3 { get; set; }
        public CityName cityName { get; set; }
        public State state { get; set; }
        public PostalCode postalCode { get; set; }
        public Country country { get; set; }
        public ShouldDisplayInStore shouldDisplayInStore { get; set; }
    }

    public class PreReleaseBuild
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public object processingError { get; set; }
        public object messagesIconAssetToken { get; set; }
        public object watchIconAssetToken { get; set; }
        public bool hasStickers { get; set; }
        public bool hasMessagesExtension { get; set; }
        public bool launchProhibited { get; set; }
        public string largeAppIconAssetToken { get; set; }
        public object watchAppIconAssetToken { get; set; }
        public bool watchOnly { get; set; }
        public int id { get; set; }
        public string buildVersion { get; set; }
        public string trainVersion { get; set; }
        public long uploadDate { get; set; }
        public string iconUrl { get; set; }
        public string iconAssetToken { get; set; }
        public string appName { get; set; }
        public string platform { get; set; }
        public bool betaEntitled { get; set; }
        public bool exceededFileSizeLimit { get; set; }
        public bool wentLiveWithVersion { get; set; }
        public bool processing { get; set; }
        public object processingState { get; set; }
        public string exportComplianceState { get; set; }
    }

    public class RatingsReset: CommonSharedValues
    {
        public bool value { get; set; }
    }

    public class DescriptorSharedValues
    {
        public string name { get; set; }
        public string level { get; set; }
        public int rank { get; set; }
    }

    public class Ratings: CommonSharedValues
    {
        public bool isEmptyValue { get; set; }
        public List<DescriptorSharedValues> nonBooleanDescriptors { get; set; }
        public List<DescriptorSharedValues> booleanDescriptors { get; set; }
        public object ageBandMin { get; set; }
        public object ageBandMax { get; set; }
        public object ageBand { get; set; }
        public List<string> allRatingLevels { get; set; }
        public string rating { get; set; }
        public Dictionary<string, string> countryRatings { get; set; }
    }

    public class PromotionalText: CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class Scaled: CommonSharedValues
    {
        public bool value { get; set; }
    }

    public class ImageSharedValues
    {
        public int? height { get; set; }
        public int? sortOrder { get; set; }
        public string checksum { get; set; }
        public int? width { get; set; }
        public int? size { get; set; }
        public string assetToken { get; set; }
        public string type { get; set; }
        public string originalFileName { get; set; }
    }

    public class ScreenshotValue: CommonSharedValues
    {
        public ImageSharedValues value { get; set; }
    }

    public class Trailers: CommonSharedValues
    {
        public List<TrailerValue> value { get; set; }
    }

    public class MessagesScaled: CommonSharedValues
    {
        public bool value { get; set; }
    }

    public class MessagesScreenshots: CommonSharedValues
    {
        public List<ScreenshotValue> value { get; set; }
    }

    public class DisplayFamiliesValue
    {
        public string name { get; set; }
        public Scaled scaled { get; set; }
        public Screenshots screenshots { get; set; }
        public Trailers trailers { get; set; }
        public MessagesScaled messagesScaled { get; set; }
        public MessagesScreenshots messagesScreenshots { get; set; }
    }

    public class DisplayFamilies: CommonSharedValues
    {
        public List<DisplayFamiliesValue> value { get; set; }
    }

    public class DetailsValue
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public Description description { get; set; }
        public string language { get; set; }
        public string detailId { get; set; }
        public ReleaseNotes releaseNotes { get; set; }
        public Keywords keywords { get; set; }
        public PromotionalText promotionalText { get; set; }
        public DisplayFamilies displayFamilies { get; set; }
        public bool canDeleteLocale { get; set; }
        public SupportUrl supportUrl { get; set; }
        public MarketingUrl marketingUrl { get; set; }
    }

    public class GetAppVersionResponseObject
    {
        public AppVersion data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
