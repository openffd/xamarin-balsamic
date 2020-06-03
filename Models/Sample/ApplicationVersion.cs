using AppKit;
using Balsamic.Enums;
using Foundation;
using Newtonsoft.Json;
using System.Collections.Generic;
using static Balsamic.Enums.ApplicationVersion.Status;

namespace Balsamic.Models.Sample
{
    sealed class ApplicationVersion : NSObject, ILeadingContentListOutlineViewNodePayload
    {
        internal static string ResourcePath = "Data/Sample/ApplicationVersion/ApplicationVersion";

        [JsonProperty(PropertyName = "app-id")]
        public string AppId { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public List<ApplicationVersionMetadata> Metadata { get; set; }

        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "review")]
        public ApplicationVersionReview Review { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "store-id")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string VersionString { get; set; }

        #region Computed Fields

        internal Enums.ApplicationVersion.Status Status => State switch
        {
            "developerRemovedFromSale"  => DeveloperRemovedFromSale,
            "prepareForUpload"          => PrepareForSubmission,
            _                           => Unknown,
        };

        internal string StateText => Status.String();

        #endregion

        #region ILeadingContentListOutlineViewNodePayload

        public LeadingContentListOutlineViewNodeType NodeType => LeadingContentListOutlineViewNodeType.ApplicationVersion;

        public NSImage Image => Status.Image();

        public string Title => $"{VersionString} {StateText}";

        public string Subtitle => string.Empty;

        #endregion
    }
}
