using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using static Balsamic.Models.Sample.ApplicationVersionMetadataConverter;
using static Newtonsoft.Json.JsonConvert;
using static Newtonsoft.Json.NullValueHandling;
using static Newtonsoft.Json.Required;

namespace Balsamic.Models.Sample
{
    internal sealed partial class ApplicationVersionMetadata : NSObject
    {
        [JsonProperty(PropertyName = "app-store-name", NullValueHandling = Ignore, Required = DisallowNull)]
        public string? AppStoreName { get; set; }

        [JsonProperty(PropertyName = "app-store-subtitle")]
        public string? AppStoreSubtitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public new string? Description { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string? Keywords { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string? Lang { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string? StoreId { get; set; }

        [JsonProperty(PropertyName = "support-url")]
        public string? SupportUrl { get; set; }

        [JsonProperty(PropertyName = "version-id")]
        public string? VersionId { get; set; }
    }

    partial class ApplicationVersionMetadata
    {
        internal static ApplicationVersionMetadata? FromJson(string json)
        {
            return DeserializeObject<ApplicationVersionMetadata>(json, Settings);
        }
    }

    static class ApplicationVersionMetadataSerializer
    {
        public static string ToJson(this ApplicationVersionMetadata self)
        {
            return SerializeObject(self, Settings);
        }
    }

    static class ApplicationVersionMetadataConverter
    {
        internal static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }, },
        };
    }
}
