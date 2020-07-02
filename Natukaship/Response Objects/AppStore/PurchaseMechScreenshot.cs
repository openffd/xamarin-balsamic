using System.Collections.Generic;

namespace Natukaship
{
    public class PurchaseMechScreenshot
    {
        public List<PurchaseMerchScreenshotImage> images { get; set; }
        public bool showByDefault { get; set; }
        public bool isActive { get; set; }
    }

    public class PurchaseMerchScreenshotImage
    {
        public object id { get; set; }
        public PurchaseMerchImage image { get; set; }
        public string status { get; set; }
    }

    public class PurchaseMerchImage : CommonSharedValues
    {
        public PurchaseMerchImageValue value { get; set; }
    }

    public class PurchaseMerchImageValue
    {
        public string assetToken { get; set; }
        public string originalFileName { get; set; }
        public string checksum { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
    }
}
