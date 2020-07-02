using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class Device
    {
        // The ID given from the developer portal. You'll probably not need it.
        // @example
        //   "XJXGVS46MW"
        public string deviceId { get; set; }

        // The name of the device, must be 50 characters or less.
        // @example
        //   "Felix Krause's iPhone 6"
        public string name { get; set; }

        // The UDID of the device
        // @example
        //   "4c24a7ee5caaa4847f49aaab2d87483053f53b65"
        public string deviceNumber { get; set; }

        // The platform of the device. This is probably always "ios"
        // @example
        //   "ios"
        public string devicePlatform { get; set; }

        // Model (can be nil)
        // @example
        //  'iPhone 6', 'iPhone 4 GSM'
        public string model { get; set; }

        // Device type
        // @example
        //   'watch'  - Apple Watch
        //   'ipad'   - iPad
        //   'iphone' - iPhone
        //   'ipod'   - iPod
        //   'tvOS'   - Apple TV
        public string deviceClass { get; set; }

        // Status of the device. "c" for enabled devices, "r" for disabled devices.
        // @example
        //   "c"
        public string status { get; set; }

        public DateTime dateAdded { get; set; }
        public DateTime dateCreated { get; set; }
        public string serialNumber { get; set; }
    }

    public class ListDevicesResponseObject
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
        public List<Device> devices { get; set; }
    }
}
