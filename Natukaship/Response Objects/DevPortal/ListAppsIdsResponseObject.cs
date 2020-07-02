using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class ListAppsIdsResponseObject
    {
        public string responseId { get; set; }
        public bool isAdmin { get; set; }
        public List<AppId> appIds { get; set; }
        public DateTime creationTimestamp { get; set; }
        public int pageNumber { get; set; }
        public string requestUrl { get; set; }
        public int resultCode { get; set; }
        public string protocolVersion { get; set; }
        public int totalRecords { get; set; }
        public bool isMember { get; set; }
        public string userLocale { get; set; }
        public bool isAgent { get; set; }
        public int pageSize { get; set; }
    }
}
