using Foundation;
using Newtonsoft.Json;

namespace Balsamic.Models.Sample
{
    sealed class ApplicationVersionReview : NSObject
    {
        [JsonProperty(PropertyName = "email")]
        public string? Email { get; set; }

        [JsonProperty(PropertyName = "first-name")]
        public string? FirstName { get; set; }

        [JsonProperty(PropertyName = "last-name")]
        public string? LastName { get; set; }

        [JsonProperty(PropertyName = "needs-credentials")]
        public bool NeedsCredentials { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string? Phone { get; set; }
    }
}
