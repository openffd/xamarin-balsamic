using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class RevokeCertificateResponseObject
    {
        public DateTime creationTimestamp { get; set; }
        public int resultCode { get; set; }
        public string userLocale { get; set; }
        public string protocolVersion { get; set; }
        public string requestUrl { get; set; }
        public string responseId { get; set; }
        public bool isAdmin { get; set; }
        public bool isMember { get; set; }
        public bool isAgent { get; set; }
        public List<CertRequest> certRequests { get; set; }
    }
}
