using Foundation;
using System.IO;
using Newtonsoft.Json;
using static Balsamic.String;
using static Foundation.NSBundle;
using System.Collections.Generic;

namespace Balsamic.Models.Samples
{
    sealed class DataProvider
    {
        public Account Account { get; set; }
        public List<Application> Applications { get; set; }

        public DataProvider()
        {
            var path = MainBundle.PathForResource("Data/Account/account", FileExtension.Json.String());
            Account = JsonConvert.DeserializeObject<Account>(File.ReadAllText(path));
        }


    }

    class Account : NSObject
    {
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
}
