using Foundation;
using System.IO;
using Newtonsoft.Json;
using static Balsamic.String.FileExtension;
using static Foundation.NSBundle;
using static Newtonsoft.Json.JsonConvert;
using System.Collections.Generic;

namespace Balsamic.Models.Samples
{
    class Account : NSObject
    {
        public static string ResourcePath = "Data/Account/account";

        [JsonProperty(PropertyName = "apple-id-key")]
        public string AppleIdKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "obf-apple-id")]
        public string ObfuscatedAppleId { get; set; }

        [JsonProperty(PropertyName = "team-id-key")]
        public string TeamIdKey { get; set; }

        [JsonProperty(PropertyName = "team-name")]
        public string TeamName { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }

    class Application : NSObject
    {
        public static string ResourcePath = "Data/Application/applications";

        [JsonProperty(PropertyName = "account-id")]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "bundle-id")]
        public string BundleId { get; set; }

        [JsonProperty(PropertyName = "icon-url")]
        public string IconUrl { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "local")]
        public bool Local { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "primary-category")]
        public string PrimaryCategory { get; set; }

        [JsonProperty(PropertyName = "SKU")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "version-count")]
        public int VersionCount { get; set; }
    }

    class Version : NSObject
    {
        public static string ResourcePath = "Data/Version/versions";

        [JsonProperty(PropertyName = "app-id")]
        public string AppId { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public List<Metadata> Metadata { get; set;}

        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "review")]
        public Review Review { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string VersionString { get; set; }
    }

    public class Review: NSObject
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first-name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last-name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "needs-credentials")]
        public bool NeedsCredentials { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
    }

    public class Metadata : NSObject
    {
        [JsonProperty(PropertyName = "app-store-name")]
        public string AppStoreName { get; set; }

        [JsonProperty(PropertyName = "app-store-subtitle")]
        public string AppStoreSubtitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public new string Description { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "support-url")]
        public string SupportUrl { get; set; }

        [JsonProperty(PropertyName = "version-id")]
        public string VersionId { get; set; }
    }

    sealed class DataProvider
    {
        public Account Account { get; set; }
        public List<Application> Applications { get; set; }
        public List<Version> Versions { get; set; }

        public DataProvider()
        {
            var accountJsonPath = MainBundle.PathForResource(Account.ResourcePath, Json.String());
            Account = DeserializeObject<Account>(File.ReadAllText(accountJsonPath));

            var applicationsJsonPath = MainBundle.PathForResource(Application.ResourcePath, Json.String());
            Applications = DeserializeObject<List<Application>>(File.ReadAllText(applicationsJsonPath));

            var versionsJsonPath = MainBundle.PathForResource(Version.ResourcePath, Json.String());
            Versions = DeserializeObject<List<Version>>(File.ReadAllText(versionsJsonPath));
        }
    }
}
