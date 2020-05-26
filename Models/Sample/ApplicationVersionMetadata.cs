using Foundation;
using Newtonsoft.Json;

namespace Balsamic.Models.Sample
{
    sealed class ApplicationVersionMetadata : NSObject
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
}
