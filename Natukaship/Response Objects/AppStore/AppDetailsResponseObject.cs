using System.Collections.Generic;

namespace Natukaship
{
    public class PrimaryCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class PrimaryFirstSubCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class SecondaryCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class SecondarySecondSubCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class LanguageName: CommonSharedValues
    {
        public string value { get; set; }
        public int maxLength { get; set; } = 30;
        public int minLength { get; set; } = 2;
    }

    public class PrimarySecondSubCategory: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class SubscriptionStatusUrl: CommonSharedValues
    {
        public int minLength { get; set; }
        public object value { get; set; }
        public int maxLength { get; set; }
    }

    public class GracRatingClassificationNumber: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class BundleId: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class PrivacyPolicy: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Name
    {
        public int minLength { get; set; } = 2;
        public string value { get; set; }
        public bool isEditable { get; set; } = true;
        public bool isRequired { get; set; } = true;
        public List<string> errorKeys { get; set; }
        public int maxLength { get; set; } = 30;
    }

    public class Subtitle: CommonSharedValues
    {
        public int minLength { get; set; } = 0;
        public string value { get; set; }
        public int maxLength { get; set; } = 30;
    }

    public class PrivacyPolicyUrl: CommonSharedValues
    {
        public int minLength { get; set; } = 0;
        public string value { get; set; }
        public int maxLength { get; set; } = 255;
    }

    public class Language
    {
        public string localeCode { get; set; }
        public PrivacyPolicy privacyPolicy { get; set; }
        public bool canDeleteLocale { get; set; }
        public LanguageName name { get; set; }
        public Subtitle subtitle { get; set; }
        public PrivacyPolicyUrl privacyPolicyUrl { get; set; }
        public string language { get; set; }
    }

    public class LocalizedMetadata: CommonSharedValues
    {
        public List<Language> value { get; set; }
    }

    public class BundleIdSuffix: CommonSharedValues
    {
        public object value { get; set; }
    }

    public class PrimaryLocaleCode: CommonSharedValues
    {
        public string value { get; set; }
    }

    public class License: CommonSharedValues
    {
        public List<object> countries { get; set; }
        public bool isEmptyValue { get; set; }
        public object EULAText { get; set; }
    }

    public class AvailableBundleId
    {
        public string name { get; set; }
        public string bundleId { get; set; }
    }

    public class AppDetailsResponseObject
    {
        public string statusCode { get; set; }
        public AppDetail data { get; set; }
        public Messages messages { get; set; }
    }
}
