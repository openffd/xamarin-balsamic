using System;
namespace Natukaship
{
    public class SubmitCertificateRequestResponseObject
    {
        public string responseId { get; set; }
        public bool isMember { get; set; }
        public string protocolVersion { get; set; }
        public string requestUrl { get; set; }
        public bool isAdmin { get; set; }
        public CertRequest certRequest { get; set; }
        public string userLocale { get; set; }
        public int resultCode { get; set; }
        public bool isAgent { get; set; }
        public DateTime creationTimestamp { get; set; }
    }
}
