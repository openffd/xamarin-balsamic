using System.Collections.Generic;

namespace Natukaship
{
    public class UploadFileResponseObject
    {
        public string token { get; set; }
        public string tokenType { get; set; }
        public string type { get; set; }
        public object fileType { get; set; }
        public object contentType { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public bool hasAlpha { get; set; }
        public long dsId { get; set; }
        public object warningCodes { get; set; }
        public string suggestionCode { get; set; }
        public string nonLocalizedMessage { get; set; }
        public string localizedMessage { get; set; }
        public object descriptionDoc { get; set; }
        public int? length { get; set; }
        public string md5 { get; set; }
        public object imgSvcTemplateUrl { get; set; }

        // if contains error
        public int? statusCode { get; set; }
        public List<string> errorCodes { get; set; }

        // trailers
        public List<UploadFileResponse> responses { get; set; }
    }

    public class UploadFileResponse
    {
        public string url { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public string token { get; set; }
        public string groupToken { get; set; }
        public string blobString { get; set; }
        public string descriptionDoc { get; set; }
    }
}
