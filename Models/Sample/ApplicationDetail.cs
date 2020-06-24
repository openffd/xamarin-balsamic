using System;
using AppKit;
using Foundation;
using Newtonsoft.Json;

namespace Balsamic.Models.Sample
{
    sealed class ApplicationDetail : NSObject, ILeadingContentListOutlineViewNodePayload
    {
        internal static string ResourcePath = "Data/Sample/ApplicationDetail/ApplicationDetail";

        [JsonProperty(PropertyName = "account-id")]
        public string? AccountId { get; set; }

        [JsonProperty(PropertyName = "bundle-id")]
        public string? BundleId { get; set; }

        [JsonProperty(PropertyName = "icon-url")]
        public string? IconUrl { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string? Language { get; set; }

        [JsonProperty(PropertyName = "local")]
        public bool Local { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string? Platform { get; set; }

        [JsonProperty(PropertyName = "primary-category")]
        public string? PrimaryCategory { get; set; }

        [JsonProperty(PropertyName = "SKU")]
        public string? Sku { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string? StoreId { get; set; }

        [JsonProperty(PropertyName = "version-count")]
        public int VersionCount { get; set; }

        #region ILeadingContentListOutlineViewNodePayload

        public LeadingContentListOutlineViewNodeType NodeType => LeadingContentListOutlineViewNodeType.ApplicationDetail;

        public NSImage Image
        {
            get {
                if (IconUrl == null || IconUrl.Length == 0)
                    return Balsamic.Image.AppIconGrid;

                string urlString = Uri.EscapeUriString(IconUrl);
                NSUrl url = new NSUrl(urlString);
                NSImage image = new NSImage(url);
                return image;
            }
        }

        public string? Title => Name;

        public string? Subtitle => BundleId;

        #endregion
    }
}
