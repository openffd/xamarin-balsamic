using System;
namespace Natukaship
{
    public class DeleteProvisioningProfileResponseObject
    {
        public DateTime creationTimestamp { get; set; }
        public bool isMember { get; set; }
        public string protocolVersion { get; set; }
        public string requestUrl { get; set; }
        public bool isAdmin { get; set; }
        public string userLocale { get; set; }
        public int resultCode { get; set; }
        public bool isAgent { get; set; }
        public string responseId { get; set; }
    }
}
