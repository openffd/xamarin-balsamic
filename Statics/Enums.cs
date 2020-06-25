using AppKit;
using System;
using System.ComponentModel;
using static AppKit.NSColor;
using static AppKit.NSImage;
using static AppKit.NSImageName;
using static Balsamic.Enums.ApplicationVersion.Status;

namespace Balsamic.Enums
{
    internal static class ApplicationVersion
    {
        internal enum Status
        {
            [Description("Developer Removed from Sale")]    DeveloperRemovedFromSale,
            [Description("Prepare for Submission")]         PrepareForSubmission,
            [Description("Ready for Sale")]                 ReadyForSale,
            [Description("Unknown")]                        Unknown,
        }

        internal static string ColorString(this Status status)
        {
            return status switch
            {
                DeveloperRemovedFromSale    => "🔴",
                PrepareForSubmission        => "🟡",
                ReadyForSale                => "🟢",
                _                           => "⚪️",
            };
        }

        internal static NSImage Image(this Status status)
        {
            return status switch
            {
                DeveloperRemovedFromSale    => ImageNamed(StatusUnavailable),
                PrepareForSubmission        => ImageNamed(StatusPartiallyAvailable),
                ReadyForSale                => ImageNamed(StatusAvailable),
                _                           => ImageNamed(StatusNone),
            };
        }

        internal static NSColor Color(this Status status)
        {
            return status switch
            {
                DeveloperRemovedFromSale    => Red,
                PrepareForSubmission        => Yellow,
                ReadyForSale                => Green,
                _                           => White,
            };
        }
	}
}
