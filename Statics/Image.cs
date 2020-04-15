using AppKit;
using System.ComponentModel;

namespace Balsamic
{
    static class Image
    {
        static class System
        {
            internal enum Name
            {
                [Description("NSGoForwardTemplate")]
                GoForwardTemplate,
                [Description("NSStatusUnavailable")]
                StatusUnavailable,
                [Description("NSUserAccounts")]
                UserAccounts,
            }
        }

        internal static NSImage GoForwardTemplate => NSImage.ImageNamed(System.Name.GoForwardTemplate.String());
        internal static NSImage StatusUnavailable => NSImage.ImageNamed(System.Name.StatusUnavailable.String());
        internal static NSImage UserAccounts => NSImage.ImageNamed(System.Name.UserAccounts.String());
    }
}
