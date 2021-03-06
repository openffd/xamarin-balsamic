﻿using AppKit;
using System.ComponentModel;

namespace Balsamic
{
    internal static class Image
    {
        private static class SystemImage
        {
            internal enum Name
            {
                [Description("NSGoForwardTemplate")]    GoForwardTemplate,
                [Description("NSStatusUnavailable")]    StatusUnavailable,
                [Description("NSUserAccounts")]         UserAccounts,
            }
        }

        internal static NSImage AppIconGrid => NSImage.ImageNamed("AppIconGrid");
        internal static NSImage AppLogo => NSImage.ImageNamed("AppLogo");
        internal static NSImage GoForwardTemplate => NSImage.ImageNamed(SystemImage.Name.GoForwardTemplate.String());
        internal static NSImage StatusUnavailable => NSImage.ImageNamed(SystemImage.Name.StatusUnavailable.String());
        internal static NSImage UserAccounts => NSImage.ImageNamed(SystemImage.Name.UserAccounts.String());
    }
}
