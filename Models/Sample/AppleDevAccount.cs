using Foundation;
using Newtonsoft.Json;

namespace Balsamic.Models.Sample
{
    sealed class AppleDevAccount : NSObject
    {
        internal static string ResourcePath = "Data/Sample/AppleDevAccount/AppleDevAccount";

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
}
