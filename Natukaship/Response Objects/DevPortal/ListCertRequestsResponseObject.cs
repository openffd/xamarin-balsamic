using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class CertificateType
    {
        public string certificateTypeDisplayId { get; set; }
        public string name { get; set; }
        public string permissionType { get; set; }
        public string distributionType { get; set; }
        public string distributionMethod { get; set; }
        public string ownerType { get; set; }
        public int daysOverlap { get; set; }
        public int maxActive { get; set; }
        public string downloadFileName { get; set; }
    }

    public class Certificate
    {
        /// <summary>
        /// The ID given from the developer portal. You'll probably not need it.
        /// </summary>
        /// @example
        ///   "P577TH3PAA"
        public string certificateId { get; set; }

        /// <summary>
        /// Status of the certificate in number
        /// </summary>
        /// @example Issued
        ///   "4"
        public int statusCode { get; set; }

        /// <summary>
        /// Status of the certificate
        /// </summary>
        /// @example
        ///   Issued
        public string status { get; set; }

        /// <summary>
        /// The date and time when the certificate will expire
        /// in readable text
        /// </summary>
        /// @example
        ///   "Sep 28, 2020"
        public string expirationDateString { get; set; }

        /// <summary>
        /// The date and time when the certificate will expire
        /// </summary>
        /// @example
        ///   "2020-09-29T05:56:40Z"
        public DateTime expirationDateGMT { get; set; }

        public string name { get; set; }
        public string serialNumber { get; set; }
        public string expirationDate { get; set; }
        public object certificatePlatform { get; set; }
        public object displayName { get; set; }
        public CertificateType certificateType { get; set; }
        public string machineId { get; set; }
        public string machineName { get; set; }
        public bool hasAskKey { get; set; }
    }

    public class CertRequest
    {
        /// <summary>
        /// The ID given from the developer portal. You'll probably not need it.
        /// </summary>
        /// @example
        ///   "P577TH3PAA"
        public string certRequestId { get; set; }

        /// <summary>
        /// The name of the certificate
        /// </summary>
        /// @example
        ///   "SunApps GmbH"
        /// @example
        ///   "Apple Push Services"
        public string name { get; set; }

        /// <summary>
        /// Status of the certificate in number
        /// </summary>
        /// @example Issued
        ///   "4"
        public int statusCode { get; set; }

        /// <summary>
        /// Status of the certificate
        /// </summary>
        /// @example
        ///   Issued
        public string statusString { get; set; }

        /// <summary>
        /// The name of the owner
        /// </summary>
        /// @example Code Signing Identity (usually the company name)
        ///   "SunApps Gmbh"
        /// @example Push Certificate (the bundle identifier)
        ///   "tools.fastlane.app"
        public string ownerName { get; set; }

        /// <summary>
        /// The ID of the owner, that can be used to
        /// fetch more information
        /// </summary>
        /// @example
        ///   "75B83SPLAA"
        public string ownerId { get; set; }

        /// <summary>
        /// The owner type that defines if it's a push profile
        /// or a code signing identity
        /// </summary>
        /// @example Code Signing Identity
        ///   "team"
        /// @example Push Certificate
        ///   "bundle"
        public string ownerType { get; set; }

        /// <summary>
        /// Whether or not the certificate can be downloaded
        /// </summary>
        public bool canDownload { get; set; }

        /// <summary>
        /// The date and time when the certificate will expire
        /// </summary>
        /// @example
        ///   "2020-09-29T05:56:40Z"
        public DateTime expirationDate { get; set; }

        /// <summary>
        /// The date and time when the certificate will expire
        /// in readable text
        /// </summary>
        /// @example
        ///   "Sep 28, 2020"
        public string expirationDateString { get; set; }

        /// <summary>
        /// The date and time when the certificate was created
        /// </summary>
        /// @example
        ///   "2019-09-30T06:06:40Z"
        public DateTime dateCreated { get; set; }

        /// <summary>
        /// Indicates the type of this certificate
        /// which is automatically used to determine the class of
        /// the certificate. Available values listed in CERTIFICATE_TYPE_IDS
        /// </summary>
        /// @example Production Certificate
        ///   "R58UK2EWSO"
        /// @example Development Certificate
        ///   "5QPB9NHCEI"
        public string certificateTypeDisplayId { get; set; }

        public string dateRequestedString { get; set; }
        public DateTime dateRequested { get; set; }
        public bool canRevoke { get; set; }
        public string certificateId { get; set; }
        public int certificateStatusCode { get; set; }
        public int certRequestStatusCode { get; set; }
        public string serialNum { get; set; }
        public string serialNumDecimal { get; set; }
        public string typeString { get; set; }
        public CertificateType certificateType { get; set; }
        public Certificate certificate { get; set; }
        public string machineId { get; set; }
        public string machineName { get; set; }
    }

    public class ListCertRequestsResponseObject
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
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public List<CertRequest> certRequests { get; set; }
    }
}
