using System.Collections.Generic;

namespace Natukaship
{
    public class DeviceFamilies
    {
        public List<string> osx { get; set; }
        public List<string> appletvos { get; set; }
        public List<string> ios { get; set; }
    }

    public class DirectUploaderUrls
    {
        public string screenshotImageUrl { get; set; }
        public string newsstandCoverArtUrl { get; set; }
        public string arbitraryFileUrl { get; set; }
        public string resCenterFileUrl { get; set; }
        public string videoUrl { get; set; }
        public string imageUrl { get; set; }
        public string geoJsonFileUrl { get; set; }
        public string appIconImageUrl { get; set; }
    }

    public class AgeBand
    {
        public string internalName { get; set; }
        public int maxAge { get; set; }
        public int minAge { get; set; }
    }

    public class StatusLevelsCommonSharedValues
    {
        public string locKey { get; set; }
        public int level { get; set; }
    }

    public class AddOnStatusLevelsCommonSharedValues : StatusLevelsCommonSharedValues
    {
        public string group { get; set; }
    }

    public class AddOnDetailStatusLevelsCommonSharedValues : AddOnStatusLevelsCommonSharedValues
    {
        public string internalName { get; set; }
    }

    public class StatusLevels
    {
        public StatusLevelsCommonSharedValues deleted { get; set; }
        public StatusLevelsCommonSharedValues devRejected { get; set; }
        public StatusLevelsCommonSharedValues developerRemovedFromSale { get; set; }
        public StatusLevelsCommonSharedValues inExtendedReview { get; set; }
        public StatusLevelsCommonSharedValues inReview { get; set; }
        public StatusLevelsCommonSharedValues invalidBinary { get; set; }
        public StatusLevelsCommonSharedValues metadataRejected { get; set; }
        public StatusLevelsCommonSharedValues missingScreenshot { get; set; }
        public StatusLevelsCommonSharedValues parking { get; set; }
        public StatusLevelsCommonSharedValues pendingAppleRelease { get; set; }
        public StatusLevelsCommonSharedValues pendingContract { get; set; }
        public StatusLevelsCommonSharedValues pendingDeveloperRelease { get; set; }
        public StatusLevelsCommonSharedValues prepareForSubmission { get; set; }
        public StatusLevelsCommonSharedValues prepareForUpload { get; set; }
        public StatusLevelsCommonSharedValues processing { get; set; }
        public StatusLevelsCommonSharedValues readyForSale { get; set; }
        public StatusLevelsCommonSharedValues rejected { get; set; }
        public StatusLevelsCommonSharedValues removedFromSale { get; set; }
        public StatusLevelsCommonSharedValues replaced { get; set; }
        public StatusLevelsCommonSharedValues uploadReceived { get; set; }
        public StatusLevelsCommonSharedValues waitingForExportCompliance { get; set; }
        public StatusLevelsCommonSharedValues waitingForReview { get; set; }
        public StatusLevelsCommonSharedValues waitingForUpload { get; set; }
    }

    public class AddOnStatusLevels
    {
        public AddOnStatusLevelsCommonSharedValues deleted { get; set; }
        public AddOnStatusLevelsCommonSharedValues rejected { get; set; }
        public AddOnStatusLevelsCommonSharedValues missingMetadata { get; set; }
        public AddOnStatusLevelsCommonSharedValues readyToSubmit { get; set; }
        public AddOnStatusLevelsCommonSharedValues waitingForReview { get; set; }
        public AddOnStatusLevelsCommonSharedValues inReview { get; set; }
        public AddOnStatusLevelsCommonSharedValues pendingBinaryApproval { get; set; }
        public AddOnStatusLevelsCommonSharedValues readyForSale { get; set; }
        public AddOnStatusLevelsCommonSharedValues developerActionNeeded { get; set; }
        public AddOnStatusLevelsCommonSharedValues removedFromSale { get; set; }
        public AddOnStatusLevelsCommonSharedValues developerRemovedFromSale { get; set; }
        public AddOnStatusLevelsCommonSharedValues waitingForContentUpload { get; set; }
        public AddOnStatusLevelsCommonSharedValues processingContentUpload { get; set; }
        public AddOnStatusLevelsCommonSharedValues replaced { get; set; }
        public AddOnStatusLevelsCommonSharedValues pendingDeveloperRelease { get; set; }
    }

    public class AddOnDetailStatusLevels
    {
        public AddOnDetailStatusLevelsCommonSharedValues active { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues proposed { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues rejected { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues replaced { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues deleted { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues rejectionAccepted { get; set; }
        public AddOnDetailStatusLevelsCommonSharedValues waiting { get; set; }
    }

    public class BetaStatuses
    {
        public string active { get; set; }
        public string approvedInactive { get; set; }
        public string complete { get; set; }
        public string expired { get; set; }
        public string exportComplianceInvalid { get; set; }
        public string exportComplianceRejected { get; set; }
        public string inReview { get; set; }
        public string inactive { get; set; }
        public string noBetaEntitlement { get; set; }
        public string none { get; set; }
        public string notTesting { get; set; }
        public string processing { get; set; }
        public string readyToTest { get; set; }
        public string rejected { get; set; }
        public string sendInvites { get; set; }
        public string submitForReview { get; set; }
        public string waiting { get; set; }
    }

    public class LegalAppPreviewGeos
    {
        public List<string> ipadPro11 { get; set; }
        public List<string> ipadPro { get; set; }
        public List<string> desktop { get; set; }
        public List<string> appleTV { get; set; }
        public List<string> iphone58 { get; set; }
        public List<string> iphone4 { get; set; }
        public List<string> iphone65 { get; set; }
        public List<string> ipad { get; set; }
        public List<string> ipadPro129 { get; set; }
        public List<string> iphone6 { get; set; }
        public List<string> ipad105 { get; set; }
        public List<string> iphone6Plus { get; set; }
    }

    public class ImageSpecsCommonSharedValues
    {
        public string pictureType { get; set; }
        public string messagesPictureType { get; set; }
        public List<string> geos { get; set; }
    }

    public class ImageSpecs
    {
        public ImageSpecsCommonSharedValues promoArtUberBackground { get; set; }
        public ImageSpecsCommonSharedValues promoArtATVCrossoverCard { get; set; }
        public ImageSpecsCommonSharedValues promoArtProductPageMac { get; set; }
        public ImageSpecsCommonSharedValues promoArtBackgroundTitleTreatment { get; set; }
        public ImageSpecsCommonSharedValues promoArtTitleTreatment { get; set; }
        public ImageSpecsCommonSharedValues iphone6 { get; set; }
        public ImageSpecsCommonSharedValues promoArtSupportingImageryMac { get; set; }
        public ImageSpecsCommonSharedValues promoArtDiscoverTab { get; set; }
        public ImageSpecsCommonSharedValues promoArtUberLogo { get; set; }
        public ImageSpecsCommonSharedValues ipadPro { get; set; }
        public ImageSpecsCommonSharedValues desktop { get; set; }
        public ImageSpecsCommonSharedValues iphone4 { get; set; }
        public ImageSpecsCommonSharedValues iphone58 { get; set; }
        public ImageSpecsCommonSharedValues promoArtProductPageArt { get; set; }
        public ImageSpecsCommonSharedValues promoArtFeaturingArtToday { get; set; }
        public ImageSpecsCommonSharedValues iphone35 { get; set; }
        public ImageSpecsCommonSharedValues ipadPro129 { get; set; }
        public ImageSpecsCommonSharedValues promoArtTitle { get; set; }
        public ImageSpecsCommonSharedValues appleTV { get; set; }
        public ImageSpecsCommonSharedValues watchSeries4 { get; set; }
        public ImageSpecsCommonSharedValues promoArtOtherTabs { get; set; }
        public ImageSpecsCommonSharedValues iphone6Plus { get; set; }
        public ImageSpecsCommonSharedValues ipadPro11 { get; set; }
        public ImageSpecsCommonSharedValues largeAppIcon { get; set; }
        public ImageSpecsCommonSharedValues promoArtATVSubscriptionHero { get; set; }
        public ImageSpecsCommonSharedValues promoArtSupportingImagery { get; set; }
        public ImageSpecsCommonSharedValues promoArtFeaturingArtAppsGames { get; set; }
        public ImageSpecsCommonSharedValues watch { get; set; }
        public ImageSpecsCommonSharedValues newsstandCoverArt { get; set; }
        public ImageSpecsCommonSharedValues iphone65 { get; set; }
        public ImageSpecsCommonSharedValues ipad { get; set; }
        public ImageSpecsCommonSharedValues ipad105 { get; set; }
        public ImageSpecsCommonSharedValues watchAppIcon { get; set; }
    }

    public class MaxScreenshotsPerTypeNumber
    {
        public int osx { get; set; }
        public int ios { get; set; }
        public int appletvos { get; set; }
    }

    public class AppMetaDataReference
    {
        public int maxReviewNotesChars { get; set; }
        public int maxUserNameChars { get; set; }
        public int maxPasswordChars { get; set; }
        public int maxKeywordsChars { get; set; }
        public int maxFirstNameBytes { get; set; }
        public int maxLastNameBytes { get; set; }
        public int maxAppInfoEmailAddressBytes { get; set; }
        public int maxAppInfoPhoneNumberBytes { get; set; }
        public int maxURLChars { get; set; }
        public int maxBundleIdentifierChars { get; set; }
        public int maxReferenceNameBytes { get; set; }
        public MaxScreenshotsPerTypeNumber maxScreenshotsPerTypeNumber { get; set; }
        public int maxJustificationChars { get; set; }
        public int minDefaultChars { get; set; }
        public int minDescriptionChars { get; set; }
        public int minReleaseNotesChars { get; set; }
        public int minBinaryIconWidth { get; set; }
        public int maxVendorIdChars { get; set; }
        public int maxArtistNameChars { get; set; }
        public int maxVersionNumberChars { get; set; }
        public int maxCopyRightChars { get; set; }
        public int maxProviderNameChars { get; set; }
        public int maxAppDescriptionChars { get; set; }
        public int maxAppReleaseNotesChars { get; set; }
        public int maxSupportEmailChars { get; set; }
        public int maxEulaChars { get; set; }
        public int minBinaryIconHeight { get; set; }
        public int minLargeAppIconWidth { get; set; }
        public int minLargeAppIconHeight { get; set; }
        public int minScreenshotWidth { get; set; }
        public int minScreenshotHeight { get; set; }
        public int maxPromotionalTextChars { get; set; }
        public int maxSubtitleTextChars { get; set; }
        public int maxAppNameChars { get; set; }
    }

    public class PreReleaseDataReference
    {
        public int testerEmailAddressMaxLength { get; set; }
        public int testerFirstNameMinLength { get; set; }
        public int testerFirstNameMaxLength { get; set; }
        public int testerGroupNameMaxLength { get; set; }
        public int testerLastNameMinLength { get; set; }
        public int testerLastNameMaxLength { get; set; }
        public int testerGroupNameMinLength { get; set; }
        public int testerEmailAddressMinLength { get; set; }
    }

    public class HasTestingContract
    {
        public bool ios { get; set; }
        public bool mac { get; set; }
    }

    public class UserRoleOverrides
    {
        public List<string> marketing { get; set; }
        public List<string> manager { get; set; }
        public List<string> @readonly { get; set; }
        public List<string> technical { get; set; }
        public List<string> admin { get; set; }
        public List<string> encoder { get; set; }
        public List<string> finance { get; set; }
        public List<string> sales { get; set; }
    }

    public class RefData
    {
        public List<string> ratingLevels { get; set; }
        public List<string> macOSGenres { get; set; }
        public List<string> addOnTypes { get; set; }
        public Dictionary<string, List<string>> subGenreMap { get; set; }
        public List<string> contactCountries { get; set; }
        public List<string> addressCountries { get; set; }
        public Dictionary<string, bool> macOSEntitlements { get; set; }
        public Dictionary<string, string> detailLanguages { get; set; }
        public List<string> detailLocales { get; set; }
        public DeviceFamilies deviceFamilies { get; set; }
        public Dictionary<string, Dictionary<string, string>> ratingsMap { get; set; }
        public Dictionary<string, List<string>> disabledInStoreRatings { get; set; }
        public DirectUploaderUrls directUploaderUrls { get; set; }
        public string imageServiceBaseUrl { get; set; }
        public List<AgeBand> ageBands { get; set; }
        public StatusLevels statusLevels { get; set; }
        public AddOnStatusLevels addOnStatusLevels { get; set; }
        public AddOnDetailStatusLevels addOnDetailStatusLevels { get; set; }
        public BetaStatuses betaStatuses { get; set; }
        public LegalAppPreviewGeos legalAppPreviewGeos { get; set; }
        public int numPreviewsForLocaleAndSize { get; set; }
        public string appPreviewDefaultPreviewImageTime { get; set; }
        public ImageSpecs imageSpecs { get; set; }
        public AppMetaDataReference appMetaDataReference { get; set; }
        public PreReleaseDataReference preReleaseDataReference { get; set; }
        public List<string> appPreviewPlayAllowedOsAndVersions { get; set; }
        public List<string> appPreviewUploadAllowedOsAndVersions { get; set; }
        public List<string> appPreviewPlayAllowedBrowsersAndVersions { get; set; }
        public List<string> appPreviewUploadAllowedBrowsersAndVersions { get; set; }
        public HasTestingContract hasTestingContract { get; set; }
        public bool allowWatchKitTesting { get; set; }
        public bool allowExternalTesting { get; set; }
        public bool allowStartExternalTesting { get; set; }
        public bool isDevOrQA { get; set; }
        public int betaExpiryDays { get; set; }
        public long lastPromoCodeExpiration { get; set; }
        public UserRoleOverrides userRoleOverrides { get; set; }
        public Dictionary<string, string> keyValues { get; set; }
        public int testersPerGroupLimit { get; set; }
        public List<string> betaExternalPlatforms { get; set; }
        public List<string> betaInternalPlatforms { get; set; }
        public int maxInternalTesters { get; set; }
        public List<string> storePlatforms { get; set; }
        public int dailySubmissionLimit { get; set; }
        public int processedFileSizeLimitInBytes { get; set; }
        public string universalIntroUrl { get; set; }
        public bool processingIssuesExpected { get; set; }
        public bool useSFFont { get; set; }
        public List<string> subtitlePlatforms { get; set; }
        public List<string> merchAddOnPlatforms { get; set; }
        public bool gracePeriodDisabled { get; set; }
        public List<string> iosgenres { get; set; }
    }

    public class RefResponseObject
    {
        public RefData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
