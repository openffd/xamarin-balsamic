using System.Collections.Generic;

namespace Natukaship
{
    public class AppDetail : AppDetailRaw
    {
        // custom
        public Application application { get; set; }

        // attr mapping
        public List<Language> Languages => localizedMetadata.value;
        public List<string> AvailablePrimaryLocaleCodes => availablePrimaryLocaleCodes;

        public string PrimaryLocaleCode
        {
            get { return primaryLocaleCode.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                primaryLocaleCode.value = val;
            }
        }

        public string PrimaryCategory
        {
            get { return primaryCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                primaryCategory.value = val;
            }
        }

        public string PrimaryFirstSubCategory
        {
            get { return primaryFirstSubCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                primaryFirstSubCategory.value = val;
            }
        }

        public string PrimarySecondSubCategory
        {
            get { return primarySecondSubCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                primarySecondSubCategory.value = val;
            }
        }

        public string SecondaryCategory
        {
            get { return secondaryCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                secondaryCategory.value = val;
            }
        }

        public string SecondaryFirstSubCategory
        {
            get { return secondaryFirstSubCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                secondaryFirstSubCategory.value = val;
            }
        }

        public string SecondarySecondSubCategory
        {
            get { return secondarySecondSubCategory.value; }
            set
            {
                string val = PrefixApp(value);
                val = PrefixMzgenre(val);

                secondarySecondSubCategory.value = val;
            }
        }

        public string name { get; set; }
        public string subtitle { get; set; }
        public string privacyUrl { get; set; }
        public string appleTvPrivacyPolicy { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public AppDetailRaw _rawData { get; set; }
        public AppDetailRaw RawData
        {
            get
            {
                if (_rawData != null)
                    return _rawData;

                _rawData = new AppDetailRaw
                {
                    sectionWarningKeys = sectionWarningKeys,
                    sectionErrorKeys = sectionErrorKeys,
                    sectionInfoKeys = sectionInfoKeys,
                    value = value,
                    adamId = adamId,
                    localizedMetadata = localizedMetadata,
                    availablePrimaryLocaleCodes = availablePrimaryLocaleCodes,
                    primaryLocaleCode = primaryLocaleCode,
                    primaryCategory = primaryCategory,
                    primaryFirstSubCategory = primaryFirstSubCategory,
                    primarySecondSubCategory = primarySecondSubCategory,
                    secondaryCategory = secondaryCategory,
                    secondaryFirstSubCategory = secondaryFirstSubCategory,
                    secondarySecondSubCategory = secondarySecondSubCategory,
                    availableBundleIds = availableBundleIds,
                    bundleId = bundleId,
                    bundleIdSuffix = bundleIdSuffix,
                    license = license,
                    vendorId = vendorId,
                    rating = rating,
                    ageBandMinAge = ageBandMinAge,
                    ageBandMaxAge = ageBandMaxAge,
                    countryRatings = countryRatings,
                    subscriptionStatusUrl = subscriptionStatusUrl,
                    gracRatingClassificationNumber = gracRatingClassificationNumber,
                };

                return _rawData;
            }
        }

        public static AppDetail Factory(AppDetail attrs)
        {
            AppDetail obj = attrs;
            obj.UnfoldLanguages();

            return obj;
        }

        public void UnfoldLanguages()
        {
            Dictionary<string, string> additionalProperties = new Dictionary<string, string>
            {
                { "name", "name" },
                { "subtitle", "subtitle" },
                { "privacyPolicyUrl", "privacyUrl" },
                { "privacyPolicy", "appleTvPrivacyPolicy" },
            };

            foreach (var additionalProperty in additionalProperties)
            {
                this[additionalProperty.Value] = new LanguageItem(additionalProperty.Key, Languages);
            }
        }

        private string PrefixMzgenre(string val)
        {
            if (val.Contains("MZGenre"))
                return val;
            else
                return $"MZGenre.{val}";
        }

        private string PrefixApp(string val)
        {
            if (!val.Contains("Stickers"))
                return val;

            if (val.Contains("Apps"))
                return val;
            else
                return $"Apps.{val}";
        }
    }

    public class AppDetailRaw
    {
        public List<string> sectionWarningKeys { get; set; }
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public object value { get; set; }
        public string adamId { get; set; }
        public LocalizedMetadata localizedMetadata { get; set; }
        public List<string> availablePrimaryLocaleCodes { get; set; }
        public PrimaryLocaleCode primaryLocaleCode { get; set; }
        public PrimaryCategory primaryCategory { get; set; }
        public PrimaryFirstSubCategory primaryFirstSubCategory { get; set; }
        public PrimarySecondSubCategory primarySecondSubCategory { get; set; }
        public SecondaryCategory secondaryCategory { get; set; }
        public SecondaryFirstSubCategory secondaryFirstSubCategory { get; set; }
        public SecondarySecondSubCategory secondarySecondSubCategory { get; set; }
        public List<AvailableBundleId> availableBundleIds { get; set; }
        public BundleId bundleId { get; set; }
        public BundleIdSuffix bundleIdSuffix { get; set; }
        public License license { get; set; }
        public string vendorId { get; set; }
        public string rating { get; set; }
        public object ageBandMinAge { get; set; }
        public object ageBandMaxAge { get; set; }
        public Dictionary<string, string> countryRatings { get; set; }
        public SubscriptionStatusUrl subscriptionStatusUrl { get; set; }
        public GracRatingClassificationNumber gracRatingClassificationNumber { get; set; }
    }
}
